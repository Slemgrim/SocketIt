using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SocketIt
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NodeModule))]
    [AddComponentMenu("SocketIt/Module/Base Module")]
    public class BaseModule : MonoBehaviour
    {
        public List<NodeModule> ConnectedModules = new List<NodeModule>();

        public delegate void BaseEvent(NodeModule node);

        public event BaseEvent OnNodeConnected;
        public event BaseEvent OnNodeConnectedIndirect;
        public event BaseEvent OnNodeDisconnected;
        public event BaseEvent OnNodeDisconnectedIndirect;

        public NodeModule Node;

        public void Reset()
        {
            Node = GetComponent<NodeModule>();
        }

        public void Awake()
        {
            Node.OnConnectChild += OnConnectChild;
            Node.OnDisconnectChild += OnDisconnectChild;
        }

        private void OnConnectChild(NodeModule node)
        {
            List<NodeModule> affectedNodes = GetAllChilds(node);
            affectedNodes.Add(node);

            foreach (NodeModule affectedNode in affectedNodes)
            {
                Activate(affectedNode);
                AddToConnectedModules(affectedNode);

                if(affectedNode == node)
                {
                    if(OnNodeConnected != null)
                    {
                        OnNodeConnected(affectedNode);
                    }
                } else
                {
                    if (OnNodeConnectedIndirect != null)
                    {
                        OnNodeConnectedIndirect(affectedNode);
                    }
                }
            }
        }

        private void OnDisconnectChild(NodeModule node)
        {
            List<NodeModule> affectedNodes = GetAllChilds(node);
            affectedNodes.Add(node);
            
            foreach(NodeModule affectedNode in affectedNodes)
            {
                Deactivate(affectedNode);
                RemoveFromConnectedModules(affectedNode);

                if (affectedNode == node)
                {
                    if (OnNodeDisconnected != null)
                    {
                        OnNodeDisconnected(affectedNode);
                    }
                }
                else
                {
                    if (OnNodeDisconnectedIndirect != null)
                    {
                        OnNodeDisconnectedIndirect(affectedNode);
                    }
                }
            }
        }

        private List<NodeModule> GetAllChilds(NodeModule node)
        {
            List<NodeModule> childs = new List<NodeModule>();

            childs = new List<NodeModule>(node.ChildNodes);

            foreach (NodeModule child in node.ChildNodes)
            {
                childs.AddRange(GetAllChilds(child));
            }

            return childs;
        }

        private void AddToConnectedModules(NodeModule node)
        {
            if (!ConnectedModules.Contains(node))
            {
                ConnectedModules.Add(node);
            }
        }

        private void RemoveFromConnectedModules(NodeModule node)
        {
            if (ConnectedModules.Contains(node))
            {
                ConnectedModules.Remove(node);
            }
        }

        private void Activate(NodeModule node)
        {
            IBaseConnector connector = node.GetComponent<IBaseConnector>();
            if(connector != null)
            {
                connector.Connect(this);
            }
        }

        private void Deactivate(NodeModule node)
        {
            IBaseConnector connector = node.GetComponent<IBaseConnector>();
            if (connector != null)
            {
                connector.Disconnect(this);
            }
        }
    }   
}

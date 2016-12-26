using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SocketIt
{
    public class BaseModule : MonoBehaviour
    {
        public List<ModuleNode> ConnectedModules = new List<ModuleNode>();

        public delegate void BaseEvent(ModuleNode node);

        public event BaseEvent OnNodeConnected;
        public event BaseEvent OnNodeConnectedIndirect;
        public event BaseEvent OnNodeDisconnected;
        public event BaseEvent OnNodeDisconnectedIndirect;

        private ModuleNode node;

        public void Start()
        {
            node = GetComponent<ModuleNode>();
            if (node != null)
            {
                node.OnConnectChild += OnConnectChild;
                node.OnDisconnectChild += OnDisconnectChild;
            }
        }

        private void OnConnectChild(ModuleNode node)
        {
            List<ModuleNode> affectedNodes = GetAllChilds(node);
            affectedNodes.Add(node);

            foreach (ModuleNode affectedNode in affectedNodes)
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

        private void OnDisconnectChild(ModuleNode node)
        {
            List<ModuleNode> affectedNodes = GetAllChilds(node);
            affectedNodes.Add(node);
            
            foreach(ModuleNode affectedNode in affectedNodes)
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

        private List<ModuleNode> GetAllChilds(ModuleNode node)
        {
            List<ModuleNode> childs = new List<ModuleNode>();

            childs = new List<ModuleNode>(node.ChildNodes);

            foreach (ModuleNode child in node.ChildNodes)
            {
                childs.AddRange(GetAllChilds(child));
            }

            return childs;
        }

        private void AddToConnectedModules(ModuleNode node)
        {
            if (!ConnectedModules.Contains(node))
            {
                ConnectedModules.Add(node);
            }
        }

        private void RemoveFromConnectedModules(ModuleNode node)
        {
            if (ConnectedModules.Contains(node))
            {
                ConnectedModules.Remove(node);
            }
        }

        private void Activate(ModuleNode node)
        {
            IBaseConnector connector = node.GetComponent<IBaseConnector>();
            if(connector != null)
            {
                connector.Connect(this);
            }
        }

        private void Deactivate(ModuleNode node)
        {
            IBaseConnector connector = node.GetComponent<IBaseConnector>();
            if (connector != null)
            {
                connector.Disconnect(this);
            }
        }
    }   
}

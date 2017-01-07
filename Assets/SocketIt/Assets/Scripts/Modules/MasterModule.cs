using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SocketIt
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NodeModule))]
    [AddComponentMenu("SocketIt/Module/Master Module")]
    public class MasterModule : MonoBehaviour, ISnapValidator
    {
        public List<SlaveModule> ConnectedSlaves = new List<SlaveModule>();

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
                SlaveModule slave = affectedNode.GetComponent<SlaveModule>();
                slave.Master = this;
                AddToConnectedModules(slave);

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
                SlaveModule slave = affectedNode.GetComponent<SlaveModule>();
                slave.Master = null;
                Deactivate(affectedNode);
                RemoveFromConnectedModules(slave);

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

        private void AddToConnectedModules(SlaveModule slave)
        {
            if (!ConnectedSlaves.Contains(slave))
            {
                ConnectedSlaves.Add(slave);
            }
        }

        private void RemoveFromConnectedModules(SlaveModule slave)
        {
            if (ConnectedSlaves.Contains(slave))
            {
                ConnectedSlaves.Remove(slave);
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

        public bool Validate(Snap snap)
        {
            MasterModule master = snap.SocketB.Module.GetComponent<MasterModule>();
            SlaveModule slave = snap.SocketA.Module.GetComponent<SlaveModule>();

            return master == null || (slave != null && slave.Master == null);
        }
    }   
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SocketIt
{
    [RequireComponent(typeof(Module))]
    [AddComponentMenu("SocketIt/Module/Node Module")]
    public class NodeModule : MonoBehaviour
    {
        public bool IsRootNode = false;

        private NodeModule parentNode;
        private List<NodeModule> childNodes = new List<NodeModule>();

        private Module module;

        public delegate void ModuleNodeEvent(NodeModule node);
        public event ModuleNodeEvent OnConnectParent;
        public event ModuleNodeEvent OnDisconnectParent;
        public event ModuleNodeEvent OnConnectChild;
        public event ModuleNodeEvent OnDisconnectChild;

        public NodeModule ParentNode
        {
            get
            {
                return parentNode;
            }
        }

        public List<NodeModule> ChildNodes
        {
            get
            {
                return childNodes;
            }
        }

        public Module Module
        {
            get
            {
                return module;
            }
        }

        public void Start()
        {
            module = GetComponent<Module>();
            module.OnConnect += OnConnect;
            module.OnDisconnect += OnDisconnect;
        }

        private void OnConnect(Connection connection)
        {
            NodeModule otherNode = connection.SocketB.Module.GetComponent<NodeModule>();
        
            if (IsChildNode(otherNode))
            {
                ConnectParent(otherNode);               
            } else {
                ConnectChild(otherNode);
            }
        }

        private void OnDisconnect(Connection connection)
        {
            NodeModule otherNode = connection.SocketB.Module.GetComponent<NodeModule>();
            if (otherNode == null)
            {
                return;
            }

            if (IsConnectedChildNode(otherNode))
            {
                DisconnectChild(otherNode);
            }
            else if(IsParentNode(otherNode))
            {
                DisconnectParent(otherNode);
            }
        }

        private bool IsParentNode(NodeModule otherNode)
        {
            return ParentNode == otherNode;
        }

        private bool IsConnectedChildNode(NodeModule otherNode)
        {
            return ChildNodes.Contains(otherNode);
        }

        private bool IsChildNode(NodeModule otherNode)
        {
            if (parentNode == null && !IsRootNode && (otherNode.IsRootNode || otherNode.parentNode != null))
            {
                return true;
            }

            return false;
        }

        private void ConnectChild(NodeModule otherNode)
        {
            if (childNodes.Contains(otherNode))
            {
                throw new ModuleNodeException("Node is already connected to this node");
            }

            otherNode.OnConnectChild += BroadcastChildConnect;
            otherNode.OnDisconnectChild += BroadcastChildDisconnect;

            if (OnConnectChild != null)
            {
                OnConnectChild(otherNode);
            }

            childNodes.Add(otherNode);
        }

        private void DisconnectChild(NodeModule otherNode)
        {
            otherNode.OnConnectChild -= BroadcastChildConnect;
            otherNode.OnDisconnectChild -= BroadcastChildDisconnect;

            if (OnDisconnectChild != null)
            {
                OnDisconnectChild(otherNode);
            }

            ChildNodes.Remove(otherNode);
        }

        private void ConnectParent(NodeModule otherNode)
        {
            if (OnConnectParent != null)
            {
                OnConnectParent(otherNode);
            }

            parentNode = otherNode;
        }

        private void DisconnectParent(NodeModule otherNode)
        {
            if (OnDisconnectParent != null)
            {
                OnDisconnectParent(otherNode);
            }

            parentNode = null;
        }

        private void BroadcastChildConnect(NodeModule otherNode)
        {
            if (OnConnectChild != null)
            {
                OnConnectChild(otherNode);
            }
        }

        private void BroadcastChildDisconnect(NodeModule otherNode)
        {
            if (OnDisconnectChild != null)
            {
                OnDisconnectChild(otherNode);
            }
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SocketIt
{
    public class ModuleNode : MonoBehaviour
    {
        public bool IsRootNode = false;

        private ModuleNode parentNode;
        private List<ModuleNode> childNodes = new List<ModuleNode>();

        private Module module;

        public delegate void ModuleNodeEvent(ModuleNode node);
        public event ModuleNodeEvent OnConnectParent;
        public event ModuleNodeEvent OnDisconnectParent;
        public event ModuleNodeEvent OnConnectChild;
        public event ModuleNodeEvent OnDisconnectChild;

        public ModuleNode ParentNode
        {
            get
            {
                return parentNode;
            }
        }

        public List<ModuleNode> ChildNodes
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
            ModuleNode otherNode = connection.SocketB.Module.GetComponent<ModuleNode>();
        
            if (IsChildNode(otherNode))
            {
                ConnectParent(otherNode);               
            } else {
                ConnectChild(otherNode);
            }
        }

        private void OnDisconnect(Connection connection)
        {
            ModuleNode otherNode = connection.SocketB.Module.GetComponent<ModuleNode>();
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

        private bool IsParentNode(ModuleNode otherNode)
        {
            return ParentNode == otherNode;
        }

        private bool IsConnectedChildNode(ModuleNode otherNode)
        {
            return ChildNodes.Contains(otherNode);
        }

        private bool IsChildNode(ModuleNode otherNode)
        {
            if (parentNode == null && !IsRootNode && (otherNode.IsRootNode || otherNode.parentNode != null))
            {
                return true;
            }

            return false;
        }

        private void ConnectChild(ModuleNode otherNode)
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

        private void DisconnectChild(ModuleNode otherNode)
        {
            otherNode.OnConnectChild -= BroadcastChildConnect;
            otherNode.OnDisconnectChild -= BroadcastChildDisconnect;

            if (OnDisconnectChild != null)
            {
                OnDisconnectChild(otherNode);
            }

            ChildNodes.Remove(otherNode);
        }

        private void ConnectParent(ModuleNode otherNode)
        {
            if (OnConnectParent != null)
            {
                OnConnectParent(otherNode);
            }

            parentNode = otherNode;
        }

        private void DisconnectParent(ModuleNode otherNode)
        {
            if (OnDisconnectParent != null)
            {
                OnDisconnectParent(otherNode);
            }

            parentNode = null;
        }

        private void BroadcastChildConnect(ModuleNode otherNode)
        {
            if (OnConnectChild != null)
            {
                OnConnectChild(otherNode);
            }
        }

        private void BroadcastChildDisconnect(ModuleNode otherNode)
        {
            if (OnDisconnectChild != null)
            {
                OnDisconnectChild(otherNode);
            }
        }
    }
}

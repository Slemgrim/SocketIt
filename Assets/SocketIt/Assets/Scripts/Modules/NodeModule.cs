using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SocketIt
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Module))]
    [AddComponentMenu("SocketIt/Module/Node Module")]
    public class NodeModule : MonoBehaviour
    {
        public bool IsRootNode = false;

        public NodeModule ParentNode;
        public List<NodeModule> ChildNodes = new List<NodeModule>();

        public Module Module;

        public delegate void ModuleNodeEvent(NodeModule node);
        public event ModuleNodeEvent OnConnectParent;
        public event ModuleNodeEvent OnDisconnectParent;
        public event ModuleNodeEvent OnConnectChild;
        public event ModuleNodeEvent OnDisconnectChild;

   
        public void Awake()
        {
            Module.OnConnect += OnConnect;
            Module.OnDisconnect += OnDisconnect;
        }

        public void Reset()
        {
            Module = GetComponent<Module>();
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
            if (ParentNode == null && !IsRootNode && (otherNode.IsRootNode || otherNode.ParentNode != null))
            {
                return true;
            }

            return false;
        }

        private void ConnectChild(NodeModule otherNode)
        {
            if (ChildNodes.Contains(otherNode))
            {
                throw new ModuleNodeException("Node is already connected to this node");
            }

            otherNode.OnConnectChild += BroadcastChildConnect;
            otherNode.OnDisconnectChild += BroadcastChildDisconnect;

            if (OnConnectChild != null)
            {
                OnConnectChild(otherNode);
            }

            ChildNodes.Add(otherNode);
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

            ParentNode = otherNode;
        }

        private void DisconnectParent(NodeModule otherNode)
        {
            if (OnDisconnectParent != null)
            {
                OnDisconnectParent(otherNode);
            }

            ParentNode = null;
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

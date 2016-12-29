using UnityEngine;
using System.Collections;
using System;

namespace SocketIt.Example06
{
    public class NodeEventListener : MonoBehaviour
    {
        NodeModule node;


        void Awake()
        {
            node = GetComponent<NodeModule>();
            node.OnConnectChild += OnConnectChild;
            node.OnConnectParent += OnConnectParent;
            node.OnDisconnectChild += OnDisconnectChild;
            node.OnDisconnectParent += OnDisconnectParent;
        }

        private void OnDisconnectParent(NodeModule node)
        {
            Debug.Log(string.Format(
                "Node {0} disconnected parent {1}",
                name,
                node.name
            ));
        }

        private void OnDisconnectChild(NodeModule node)
        {
            Debug.Log(string.Format(
                "Node {0} disconnected child {1}",
                name,
                node.name
            ));
        }

        private void OnConnectParent(NodeModule node)
        {
            Debug.Log(string.Format(
                "Node {0} connected parent {1}",
                name,
                node.name
            ));
        }

        private void OnConnectChild(NodeModule node)
        {
            Debug.Log(string.Format(
                "Node {0} connected child {1}",
                name,
                node.name
            ));
        }
    }
}

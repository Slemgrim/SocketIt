using UnityEngine;
using System.Collections;
using System;

namespace SocketIt.Example06
{
    public class NodeEventListener : MonoBehaviour
    {
        ModuleNode node;


        void Awake()
        {
            node = GetComponent<ModuleNode>();
            node.OnConnectChild += OnConnectChild;
            node.OnConnectParent += OnConnectParent;
            node.OnDisconnectChild += OnDisconnectChild;
            node.OnDisconnectParent += OnDisconnectParent;
        }

        private void OnDisconnectParent(ModuleNode node)
        {
            Debug.Log(string.Format(
                "Node {0} disconnected parent {1}",
                name,
                node.name
            ));
        }

        private void OnDisconnectChild(ModuleNode node)
        {
            Debug.Log(string.Format(
                "Node {0} disconnected child {1}",
                name,
                node.name
            ));
        }

        private void OnConnectParent(ModuleNode node)
        {
            Debug.Log(string.Format(
                "Node {0} connected parent {1}",
                name,
                node.name
            ));
        }

        private void OnConnectChild(ModuleNode node)
        {
            Debug.Log(string.Format(
                "Node {0} connected child {1}",
                name,
                node.name
            ));
        }
    }
}

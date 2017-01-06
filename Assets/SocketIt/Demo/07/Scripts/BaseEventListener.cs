using UnityEngine;
using System.Collections;
using SocketIt;
using System;

namespace SocketIt.Example07
{
    public class BaseEventListener : MonoBehaviour
    {
        private MasterModule baseModule;

        public void Awake()
        {
            baseModule = GetComponent<MasterModule>();
            baseModule.OnNodeConnected += OnNodeConnected;
            baseModule.OnNodeConnectedIndirect += OnNodeConnectedIndirect;
            baseModule.OnNodeDisconnected += OnNodeDisconnected;
            baseModule.OnNodeDisconnectedIndirect += OnNodeDisconnectedIndirect;

        }

        private void OnNodeDisconnectedIndirect(NodeModule node)
        {
            Debug.Log(string.Format(
                "Node {0} disconnected indirect to {1}",
                node.name,
                name
            ));
        }

        private void OnNodeDisconnected(NodeModule node)
        {
            Debug.Log(string.Format(
                "Node {0} disconnected to {1}",
                node.name,
                name
            ));
        }

        private void OnNodeConnectedIndirect(NodeModule node)
        {
            Debug.Log(string.Format(
                "Node {0} connected indirect to {1}",
                node.name,
                name
            ));
        }

        private void OnNodeConnected(NodeModule node)
        {
            Debug.Log(string.Format(
                "Node {0} connected to {1}",
                node.name,
                name
            ));
        }
    }
}

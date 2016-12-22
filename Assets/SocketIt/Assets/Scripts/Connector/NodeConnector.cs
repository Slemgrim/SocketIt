using UnityEngine;
using System.Collections;
using System;

namespace SocketIt
{
    public class NodeConnector : MonoBehaviour, IModuleConnector, ISnapValidator
    {
        public event ModuleConnectEvent OnConnectEnd;
        public event ModuleConnectEvent OnConnectStart;
        public event ModuleConnectEvent OnDisconnectEnd;
        public event ModuleConnectEvent OnDisconnectStart;

        public bool OnlyConnectToRootNodes = true;
        public bool ConnectToChildNodes = true;

        private Module Module;

        public void Start()
        {
            Module = GetComponent<Module>();
            Module.OnConnect += Connect;
            Module.OnDisconnect += Disconnect;
        }

        private void Connect(Connection connection)
        {
            if (OnConnectStart != null)
            {
                OnConnectStart(connection);
            }

            if (connection.SocketB.Module.transform.parent != connection.SocketA.Module.transform)
            {
                connection.SocketA.Module.transform.SetParent(connection.SocketB.Module.transform);
            }

            if (OnConnectEnd != null)
            {
                OnConnectEnd(connection);
            }
        }

        private void Disconnect(Connection connection)
        {
            if (OnDisconnectStart != null)
            {
                OnDisconnectStart(connection);
            }

            if (connection.SocketA.Module.transform.parent == connection.SocketB.Module.transform)
            {
                connection.SocketA.Module.transform.SetParent(null);
            }

            if (OnDisconnectEnd != null)
            {
                OnDisconnectEnd(connection);
            }
        }

        public bool Validate(Snap snap)
        {
            ModuleNode node = snap.SocketB.Socket.Module.GetComponent<ModuleNode>();
            if (OnlyConnectToRootNodes && node != false && node.IsRootNode)
            {
                return true;
            }

            if (ConnectToChildNodes && node != false && node.ParentNode != null)
            {
                return true;
            }

            return false;
        }
    }
}
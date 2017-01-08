using UnityEngine;
using System.Collections;
using System;

namespace SocketIt
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Module))]
    [AddComponentMenu("SocketIt/Connector/Transform Connector")]
    public class TransformConnector : MonoBehaviour, IModuleConnector
    {
        public event ModuleConnectEvent OnConnectEnd;
        public event ModuleConnectEvent OnConnectStart;
        public event ModuleConnectEvent OnDisconnectEnd;
        public event ModuleConnectEvent OnDisconnectStart;

        private Module Module;

        public void Start()
        {
            Module = GetComponent<Module>();
            Module.OnModuleConnected += Connect;
            Module.OnModuleDisconnected += Disconnect;
        }

        private void Connect(Connection connection)
        {
            if(OnConnectStart != null)
            {
                OnConnectStart(connection);
            }

            if (connection.Connectee.Module.transform.parent != connection.Connector.Module.transform)
            {
                connection.Connector.Module.transform.SetParent(connection.Connectee.Module.transform);
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

            if (connection.Connector.Module.transform.parent == connection.Connectee.Module.transform)
            {
                connection.Connector.Module.transform.SetParent(null);
            }

            if (OnDisconnectEnd != null)
            {
                OnDisconnectEnd(connection);
            }
        }
    }
}
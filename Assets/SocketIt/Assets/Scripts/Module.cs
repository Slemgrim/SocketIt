using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace SocketIt
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SocketIt/Module/Module")]
    public class Module : MonoBehaviour
    {
        public List<Socket> Sockets = new List<Socket>();

        [HideInInspector]
        public Composition Composition = null;

        public List<ISnapValidator> SnapValidators;

        [System.Serializable]
        public class SnapEvent : UnityEvent<Snap> { }
        public SnapEvent OnSnap;

        [System.Serializable]
        public class ModuleEvent : UnityEvent<Connection> { }
        public ModuleEvent OnModuleConnected;
        public ModuleEvent OnModuleDisconnected; 

        [System.Serializable]
        public class CompositionEvent : UnityEvent<Composition> { }
        public CompositionEvent OnCompositionChanged;

        public void SetComposition(Composition composition)
        {
            if(composition == Composition)
            {
                return;
            }

            if(Composition != null)
            {
                Composition.OnConnectionAdded -= CompositionConnectionAdded;
                Composition.OnConnectionRemoved -= CompositionConnectionRemoved;
            }

            Composition = composition;

            if (Composition != null)
            {
                Composition.OnConnectionAdded += CompositionConnectionAdded;
                Composition.OnConnectionRemoved += CompositionConnectionRemoved;
            }

            OnCompositionChanged.Invoke(composition);
        }

        private void CompositionConnectionAdded(Connection connection)
        {
            if (connection.Connector.Module != this && connection.Connectee.Module != this)
            {
                return;
            }

            OnModuleConnected.Invoke(connection);  
        }

        private void CompositionConnectionRemoved(Connection connection)
        {
            if (connection.Connector.Module != this && connection.Connectee.Module != this)
            {
                return;
            }

            OnModuleDisconnected.Invoke(connection);
        }

        public Socket GetConnectedSocket(Socket socket)
        {
            if (Composition == null)
            {
                return null;
            }
            Connection connection = Composition.GetConnection(socket);
            if(connection != null)
            {
                return socket == connection.Connector ? connection.Connectee : connection.Connector;
            }

            return null;
        }

        public Connection GetConnection(Module otherModule)
        {
            if (Composition == null || otherModule.Composition == null || Composition != otherModule.Composition)
            {
                return null;
            }

            return Composition.GetConnection(this, otherModule);
        }

        public void Reset()
        {
            Sockets = new List<Socket>(GetComponentsInChildren<Socket>());
            foreach(Socket socket in Sockets)
            {
                socket.Module = this;
            }
        }

        public void Awake()
        {
            SnapValidators = new List<ISnapValidator>(GetComponents<ISnapValidator>());
        }

        public bool ConnectSocket(Socket connector, Socket connectee)
        {
            Composition.OnPreConnect(connector.Module, connectee.Module);
            return Composition.Connect(connector, connectee);
        }

        public bool DisconnectSocket(Socket disconnector, Socket disconnectee)
        {
            return Composition.Disconnect(disconnector, disconnectee);
        }

        public void DisconnectAll()
        {

        }

        public void DisconnectModule(Module module)
        {

            if (Composition == null)
            {
                return;
            }

            Composition.DisconnectModules(module, this);
        }

        /**
         * Removes the socket reference from the module.
         */
        public void RemoveSocket(Socket socketToRemove)
        {
           
        }

        public void SnapSockets(Snap snap)
        {
            if (!validateSnap(snap))
            {
                return;
            }

            OnSnap.Invoke(snap);
        }

        private bool validateSnap(Snap snap)
        {
            foreach (ISnapValidator validator in SnapValidators)
            {
                if (!validator.Validate(snap))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

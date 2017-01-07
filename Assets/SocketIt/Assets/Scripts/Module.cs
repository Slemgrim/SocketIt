﻿using UnityEngine;
using System.Collections.Generic;
using System;

namespace SocketIt
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SocketIt/Module/Module")]
    public class Module : MonoBehaviour
    {
        public delegate void ModuleEvent(Connection connection);
        public event ModuleEvent OnConnect;
        public event ModuleEvent OnDisconnect;

        public delegate void CompositionEvent(Composition composition);
        public event CompositionEvent OnCompositionSet;
        public event CompositionEvent OnCompositionRemoved;

        public List<Socket> Sockets = new List<Socket>();
        public Composition Composition = null;

        public List<Connection> Connections = new List<Connection>();

        internal Socket GetConnectedSocket(Socket socket)
        {
            Connection connection = GetConnection(socket);
            if(connection != null)
            {
                return connection.SocketB;
            }

            return null;
        }

        public void Awake()
        {
            Sockets = new List<Socket>(GetComponentsInChildren<Socket>());
        }
			
        public void Reset()
        {
            Sockets = new List<Socket>(GetComponentsInChildren<Socket>());
            foreach(Socket socket in Sockets)
            {
                socket.Module = this;
            }
        }

        /**
         * Makes sure that all current connections get updated when this gameObject gets destroyed
         */
        public void OnDestroy()
        {
            foreach (Connection connection in Connections)
            {
                connection.SocketB.Module.OnOtherModuleDestroyed(this);
            }
        }

        public void SetComposition(Composition composition)
        {
            if(Composition == composition)
            {
                return;
            }
            Composition = composition;

            if(composition != null && OnCompositionSet != null)
            {
                OnCompositionSet(composition);
            }
        }

        public void RemoveComposition()
        {
            Composition = null;

            if (OnCompositionRemoved != null)
            {
                OnCompositionRemoved(null);
            }
        }

        /**
         * Gets called whenever a currently connected or possible connected module gets destroyed.
         * This is neccessary to cleanup references to the destroyed module and its sockets
         */
        public void OnOtherModuleDestroyed(Module destoryedModule)
        {
            Connection connection = GetConnection(destoryedModule);
            if (connection != null)
            {
                RemoveConnection(connection.SocketA, connection.SocketB, connection.SocketB);
            }
        }

        /**
          * Gets called when a socket of a connected module gets destroyed. 
          * We need this to cleanup references to this socket and remove connections
          */
        public void OnOtherSocketDestroyed(Socket destroyedSocket)
        {
            Connection connection = GetConnection(destroyedSocket);
            if(connection != null)
            {
                RemoveConnection(connection.SocketA, connection.SocketB, connection.SocketB);
            }
        }

        public bool ConnectSocket(Socket ownSocket, Socket otherSocket, Socket initiator)
        {
            return AddConnection(ownSocket, otherSocket, initiator);
        }

        public bool DisconnectSocket(Socket ownSocket, Socket otherSocket, Socket initiator)
        {
            return RemoveConnection(ownSocket, otherSocket, initiator);
        }

        public void DisconnectAll()
        {
            List<Connection> connections = new List<Connection>(Connections);
            foreach(Connection connection in connections)
            {
                connection.SocketA.Disconnect(connection.SocketB);
            }
        }

        public void DisconnectModule(Module module)
        {
            Connection connection = GetConnection(module);
            if(connection != null)
            {
                connection.SocketA.Disconnect(connection.SocketB);
            }
        }

        /**
         * Removes the socket reference from the module.
         */
        public void RemoveSocket(Socket socketToRemove)
        {
            if (!Sockets.Contains(socketToRemove))
            {
                return;
            }

            Sockets.Remove(socketToRemove);

            Connection connection = GetConnection(socketToRemove);

            if(connection != null)
            {
                connection.SocketB.Module.OnOtherSocketDestroyed(socketToRemove);
                RemoveConnection(connection.SocketA, connection.SocketB, connection.SocketB);
            }
        }

        private bool AddConnection(Socket callingSocket, Socket otherSocket, Socket initiator)
        {
            Connection oldConnection = GetConnection(otherSocket.Module);
            if (oldConnection != null)
            {
                Debug.LogWarningFormat("Module {0} already connected to module {1}", callingSocket.Module.name, otherSocket.Module.name);
                return false;
            }

            Connection connection = new Connection();
            connection.SocketA = callingSocket;
            connection.SocketB = otherSocket;
            connection.Initiator = initiator;
            Connections.Add(connection);

            UpdateComposition(otherSocket, initiator);
            AddToComposition();

            if (OnConnect != null)
            {
                OnConnect(connection);
            }
            return true;
        }

        private void UpdateComposition(Socket otherSocket, Socket initiator)
        {
            //Destroy Composition if Module gets connected to other Composition.
            if (Composition != null && initiator.Module == this)
            {
                Composition.Delete();
            } else if (Composition != null)
            {
                return;
            }

            if (Composition == null && otherSocket.Module.Composition == null)
            {
                SetComposition(Composition.CreateNew(otherSocket.Module));
            }
             
            else if (otherSocket.Module.Composition != null)
            {
                SetComposition(otherSocket.Module.Composition);
            }
        }

        private bool RemoveConnection(Socket callingSocket, Socket otherSocket, Socket initiator)
        {
            Connection connection = GetConnection(otherSocket);
            if (connection == null)
            {
                return false;
            }

            Connections.Remove(connection);

            RemoveFromComposition();
           
            if (OnDisconnect != null)
            {
                OnDisconnect(connection);
            }

            return true;
        }

        private void AddToComposition()
        {
            Composition.AddModule(this);
            List<Module> modules = Composition.GetConnected(this);
            foreach (Module module in modules)
            {
                if(module == this)
                {
                    continue;
                }

                Composition.AddModule(module);
                module.SetComposition(Composition);
            }
        }

        private void RemoveFromComposition()
        {
            if (Composition == null)
            {
                return;
            }

            List<Module> modules = new List<Module>();
            if (!Composition.IsConnected(this, modules))
            {
                foreach (Module module in modules)
                {
                    Composition.RemoveModule(module);
                }

                if(modules.Count >= 2)
                {
                    SetComposition(Composition.CreateNew(this));
                    AddToComposition();
                }
            }
        }

        public Connection GetConnection(Module module)
        {
            foreach (Connection connection in Connections)
            {
                if (module == connection.SocketB.Module)
                {
                    return connection;
                }
            }

            return null;
        }

        public Connection GetConnection(Socket socket)
        {
            foreach (Connection connection in Connections)
            {
                if (socket == connection.SocketB || socket == connection.SocketA)
                {
                    return connection;
                }
            }

            return null;
        }

		public Connection GetConnection(Connection con)
		{
			foreach (Connection connection in Connections)
			{
				if (con.SocketB == connection.SocketB && con.SocketA == connection.SocketA)
				{
					return connection;
				}
			}

			return null;
		}
    }
}

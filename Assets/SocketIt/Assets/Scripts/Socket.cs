using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SocketIt {
    [DisallowMultipleComponent]
    [AddComponentMenu("SocketIt/Socket/Socket")]
    public class Socket : MonoBehaviour {

		public delegate void SocketEvent(Socket connector, Socket connectee);
        public event SocketEvent OnConnect;
        public event SocketEvent OnDisconnect;

        public Module Module;

        public void Awake()
        {
            if (Module == null)
            {
                throw new SocketException("Socket needs a module");
            }
        }

        public Socket GetConnectedSocket()
        {
            if (Module == null)
            {
                throw new SocketException("Socket needs a module");
            }
            return Module.GetConnectedSocket(this);
        }

        public void Connect(Socket socket)
        {
            if (!isValidSocket(socket))
            {
                throw new SocketException("Socket Already Connected");
            }

            bool isConnected = Module.ConnectSocket(this, socket);

            if (isConnected && OnConnect != null)
            {
                OnConnect(this, socket);
            }
        }

        public void Reset()
        {
            Module = GetComponentInParent<Module>();
            if(Module == null)
            {
                Debug.LogWarning("No Module component found in parent objects. Manual allocation is required");
            }

            if (!Module.Sockets.Contains(this))
            {
                Module.Sockets.Add(this);
            }
        }

        public void Disconnect(Socket socket)
        {
            Socket connectedSocket = GetConnectedSocket();
            if (connectedSocket == null || socket != connectedSocket)
            {
                return;
            }

            bool isDisconnected = Module.DisconnectSocket(this, socket);

            if (isDisconnected && OnDisconnect != null)
            {
                OnDisconnect(this, socket);
            }
        }

        public void Disconnect()
        {
            Socket connectedSocket = GetConnectedSocket();
            Disconnect(connectedSocket);
        }

        public void OnDestroy()
        {
            Module.RemoveSocket(this);
        }

        public bool IsConnected()
        {
            if(Module == null || Module.Composition == null)
            {
                return false;
            }

            foreach (Connection connection in Module.Composition.Connections)
            {
                if (connection.ContainsSocket(this))
                {
                    return true;
                }
            }

            return false;
        }

        public Connection GetConnection()
        {
            if (Module == null || Module.Composition == null)
            {
                return null;
            }

            foreach (Connection connection in Module.Composition.Connections)
            {
                if (connection.ContainsSocket(this))
                {
                    return connection;
                }
            }

            return null;
        }

        private bool isValidSocket(Socket socket)
        {
            if (socket == this)
            {
                Debug.Log("Its me");
                return false;
            }

            if (socket.Module == this)
            {
                Debug.Log("Its same module");

                return false;
            }

            /*
            Socket otherConnectedSocket = socket.GetConnectedSocket();
            if (otherConnectedSocket != null && otherConnectedSocket != this)
            {
                return false;
            }

            Socket connectedSocket = GetConnectedSocket();
            if (connectedSocket != null && connectedSocket != socket)
            {
                return false;
            }
            */

            return true;
        }
    }
}
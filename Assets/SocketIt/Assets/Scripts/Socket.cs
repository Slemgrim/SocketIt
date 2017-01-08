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

        public void OnDestroy()
        {
            Module.RemoveSocket(this);
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
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SocketIt {
    [AddComponentMenu("SocketIt/Socket/Socket")]
    public class Socket : MonoBehaviour {

		public delegate void SocketEvent(Socket socketA, Socket socketB, Socket initiator);
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

        public void Connect(Socket socket, bool callOther = true)
        {
            ConnectSocket(socket, this);
            if (callOther)
            {
                socket.Connect(this, false);
            }
        }

        public void Reset()
        {
            Module = GetComponentInParent<Module>();
            if(Module == null)
            {
                Debug.LogWarning("No Module component found in parent objects. Manual allocation is required");
            }
        }

        public void Disconnect(Socket socket, bool callOther = true)
        {
            DisconnectSocket(socket, this);
            if (callOther)
            {
                socket.Disconnect(this, false);
            }
        }

        public void OnDestroy()
        {
            Module.RemoveSocket(this);
        }

        public void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "Socket.png", true);
        }

        private void ConnectSocket(Socket socket, Socket initiator)
        {
            if (!isValidSocket(socket))
            {
                throw new SocketException("Socket Already Connected");
            }

            bool isConnected = Module.ConnectSocket(this, socket, initiator);

            if (isConnected && OnConnect != null)
            {
                OnConnect(this, socket, initiator);
            }
        }

        private void DisconnectSocket(Socket socket, Socket initiator)
        {
            Socket connectedSocket = GetConnectedSocket();
            if (connectedSocket == null || socket != connectedSocket)
            {
                return;
            }

            bool isDisconnected = Module.DisconnectSocket(this, socket, initiator);

            if (isDisconnected && OnDisconnect != null)
            {
                OnDisconnect(this, socket, initiator);
            }
        }

        private bool isValidSocket(Socket socket)
        {
            if (socket == this)
            {
                return false;
            }

            if (socket.Module == this)
            {
                return false;
            }

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

            return true;
        }
    }
}
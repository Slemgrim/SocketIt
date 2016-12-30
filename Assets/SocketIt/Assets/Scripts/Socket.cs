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

        public Socket ConnectedSocket = null;

        public bool IsConnected
        {
            get
            {
                return ConnectedSocket != null;
            }
        }
			
        public void Awake()
        {
            if (Module == null)
            {
                throw new SocketException("Socket needs a module");
            }
        }

        public void Connect(Socket socket)
        {
            ConnectSocket(socket, this);
            socket.ConnectOther(this);
        }

        public void ConnectOther(Socket socket)
        {
            ConnectSocket(socket, socket);
        }

        public void Reset()
        {
            Module = GetComponentInParent<Module>();
            if(Module == null)
            {
                Debug.LogWarning("No Module component found in parent objects. Manual allocation is required");
            }
        }

        private void ConnectSocket(Socket socket, Socket initiator)
        {
            if (IsConnected)
            {
                throw new SocketException("Socket Already Connected");
            }

            if (OnConnect != null)
            {
                OnConnect(this, socket, initiator);
            }

            ConnectedSocket = socket;
            InformModuleOfConnect(socket, initiator);
        }

        public void Disconnect(Socket socket)
        {
            DisconnectSocket(socket, this);

            socket.DisconnectOther(this);
        }

        public void DisconnectOther(Socket socket)
        {
            DisconnectSocket(socket, socket);     
        }

        private void DisconnectSocket(Socket socket, Socket initiator)
        {
            if (!IsConnected || socket != ConnectedSocket)
            {
                return;
            }

            if (OnDisconnect != null)
            {
                OnDisconnect(this, socket, initiator);
            }

            InformModuleOfDisconnect(ConnectedSocket, initiator);
            ConnectedSocket = null;
        }

        public void Clear()
        {
            ConnectedSocket = null;
        }

        public void OnDestroy()
        {
            Module.OnSocketDestroyed(this);
        }

        public void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "Socket.png", true);
        }

        private void InformModuleOfConnect(Socket otherSocket, Socket initiator)
        {
            Module.OnSocketConnect(this, otherSocket, initiator);
        }

        private void InformModuleOfDisconnect(Socket otherSocket, Socket initiator)
        {
            Module.OnSocketDisconnect(this, otherSocket, initiator);
        }

        private bool isValidSocket(Socket socket)
        {
            if (socket == this)
            {
                return false;
            }

            if (socket.Module == Module)
            {
                return false;
            }

            if (IsConnected || socket.IsConnected)
            {
                return false;
            }

            return true;
        }
    }
}
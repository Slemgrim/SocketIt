using System;
using System.Collections.Generic;
using UnityEngine;

namespace SocketIt {
	
	public class Socket : MonoBehaviour {

        public delegate void SocketEvent(Connection connection);
        public event SocketEvent OnConnect;
        public event SocketEvent OnDisconnect;

        public Module Module;

        private Socket connectedSocket = null;

        public bool IsConnected
        {
            get
            {
                return connectedSocket != null;
            }
        }

        public Socket ConnectedSocket
        {
            get
            {
                return connectedSocket;
            }
        }

        public void Awake()
        {
            CheckModule();
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

        private void ConnectSocket(Socket socket, Socket initiator)
        {
            if (IsConnected)
            {
                throw new SocketException("Socket Already Connected");
            }

            if (OnConnect != null)
            {
                OnConnect(new Connection(this, socket, initiator));
            }

            connectedSocket = socket;
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
            if (!IsConnected || socket != connectedSocket)
            {
                return;
            }

            if (OnDisconnect != null)
            {
                OnDisconnect(new Connection(this, socket, initiator));
            }

            InformModuleOfDisconnect(connectedSocket, initiator);
            connectedSocket = null;
        }

        public void Clear()
        {
            connectedSocket = null;
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

        private void CheckModule()
        {
            if (Module == null)
            {
                throw new SocketException("Socket needs a module");
            }
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
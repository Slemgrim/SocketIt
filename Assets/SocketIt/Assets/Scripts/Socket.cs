using System;
using System.Collections.Generic;
using UnityEngine;

namespace SocketIt {
	
	public class Socket : MonoBehaviour {

        public Module Module;

        private Socket connectedSocket = null;

        public bool IsConnected
        {
            get
            {
                return connectedSocket != null;
            }
        }

        public void Start()
        {
            CheckModule();
        }

        public void Connect(Socket socket)
        {
            ConnectSocket(socket);
            socket.OnConnect(this);
        }

        public void OnConnect(Socket socket)
        {
            ConnectSocket(socket);
        }

        private void ConnectSocket(Socket socket)
        {
            if (IsConnected)
            {
                throw new SocketException("Socket Already Connected");
            }

            connectedSocket = socket;
            InformModuleOfConnect(socket);
        }

        public void Disconnect(Socket socket)
        {
            DisconnectSocket(socket);
            socket.OnDisconnect(this);
        }

        public void OnDisconnect(Socket socket)
        {
            DisconnectSocket(socket);     
        }

        private void DisconnectSocket(Socket socket)
        {
            if (!IsConnected || socket != connectedSocket)
            {
                return;
            }

            InformModuleOfDisconnect(connectedSocket);
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

        private void InformModuleOfConnect(Socket otherSocket)
        {
            Module.OnSocketConnect(this, otherSocket);
        }

        private void InformModuleOfDisconnect(Socket otherSocket)
        {
            Module.OnSocketDisconnect(this, otherSocket);
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
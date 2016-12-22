using System;
using System.Collections.Generic;
using UnityEngine;

namespace SocketIt
{
    public class SnapSocket : MonoBehaviour
    {
        public Module Module;
        private SnapModule SnapModule;

        public Socket Socket;

        [Range(0.0f, 180.0f)]
        public float AngleLimit = 180;

        private SnapSocket socketInReach = null;

        private bool isLocked = false;

        [SerializeField]
        public bool IsLocked
        {
            get
            {
                return isLocked;
            }
        }

        public SnapSocket SocketInReach
        {
            get
            {
                return socketInReach;
            }
        }

        public bool IsInReachOfOther
        {
            get
            {
                return socketInReach != null;
            }
        }

        public void Start()
        {
            Socket = GetComponent<Socket>();
            if(Socket == null)
            {
                throw new SocketException("SnapSocket needs a Socket component");
            }

            if (Module == null)
            {
                throw new SocketException("SnapSocket needs a module");
            }

            SnapModule = Module.GetComponent<SnapModule>();

            if (SnapModule == null)
            {
                throw new SocketException("SnapSocket is not connected to a SnapModule");

            }
        }

        public void Found(SnapSocket socket)
        {
            if (!isValidSocket(socket))
            {
                return;
            }

            SnapModule.OnSocketFound(this, socket);
            socketInReach = socket;
        }

        public void Lost(SnapSocket socket)
        {
            if (IsLocked || socket.IsLocked)
            {
                return;
            }

            if (socketInReach != socket)
            {
                return;
            }

            SnapModule.OnSocketLost(this, socket);
            socketInReach = null;
        }

        public void Clear()
        {
            socketInReach = null;
        }

        public void Lock()
        {
            isLocked = true;
            SnapModule.OnSocketLock(this);

        }

        public void Unlock()
        {
            isLocked = false;
            SnapModule.OnSocketUnlock(this);

        }

        private bool isValidSocket(SnapSocket socket)
        {
            if (socket == this)
            {
                return false;
            }

            if (IsLocked || socket.IsLocked)
            {
                return false;
            }

            if (socket.Module == Module)
            {
                return false;
            }

            if (Socket.IsConnected || socket.Socket.IsConnected)
            {
                return false;
            }

            if (AngleLimit < 180 && !isInsideAngle(socket))
            {
                return false;
            }

            if (socketInReach != null && socketInReach.Module == Module)
            {
                return false;
            }

            if (SnapModule.IsLocked || socket.SnapModule.IsLocked)
            {
                return false;
            }

            return true;
        }

        private bool isInsideAngle(SnapSocket otherSocket)
        {
            float minAngle = Math.Min(AngleLimit, otherSocket.AngleLimit);
            float connectionAngle = Vector3.Angle(transform.forward, otherSocket.transform.forward);
            return connectionAngle > 180 - minAngle / 2 && connectionAngle < 180 + minAngle / 2;
        }
    }
}
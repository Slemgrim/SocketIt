using System;
using System.Collections.Generic;
using UnityEngine;

namespace SocketIt
{
    public class SnapSocket : MonoBehaviour
    {
        public delegate void SnapSocketEvent(Snap snap);
        public event SnapSocketEvent OnSocketFound;
        public event SnapSocketEvent OnSockerLost;

        private SnapModule SnapModule;

        private Socket socket;

        private Module module;

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

        public Socket Socket
        {
            get
            {
                return socket;
            }
        }

        public Module Module
        {
            get
            {
                return module;
            }
        }

        public void Awake()
        {
            socket = GetComponent<Socket>();
            if(Socket == null)
            {
                throw new SocketException("SnapSocket needs a Socket component");
            }

            module = socket.Module;

            SnapModule = socket.Module.GetComponent<SnapModule>();

            if (SnapModule == null)
            {
                throw new SocketException("SnapSocket is not connected to a SnapModule");
            }
        }

        public void Found(SnapSocket OtherSnapSocket)
        {
            if (!isValidSocket(OtherSnapSocket))
            {
                return;
            }

            if(OnSocketFound != null)
            {
                OnSocketFound(new Snap(this, OtherSnapSocket));
            }

            SnapModule.OnSocketFound(this, OtherSnapSocket);
            socketInReach = OtherSnapSocket;
        }

        public void Lost(SnapSocket otherSnapSocket)
        {
            if (IsLocked || otherSnapSocket.IsLocked)
            {
                return;
            }

            if (socketInReach != otherSnapSocket)
            {
                return;
            }

            if (OnSockerLost != null)
            {
                OnSockerLost(new Snap(this, otherSnapSocket));
            }

            SnapModule.OnSocketLost(this, otherSnapSocket);
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

        private bool isValidSocket(SnapSocket otherSnapSocket)
        {
            if (otherSnapSocket == this)
            {
                return false;
            }

            if (IsLocked || otherSnapSocket.IsLocked)
            {
                return false;
            }

            if (socket.Module == otherSnapSocket.Socket.Module)
            {
                return false;
            }

            if (Socket.IsConnected || otherSnapSocket.Socket.IsConnected)
            {
                return false;
            }

            if (AngleLimit < 180 && !isInsideAngle(otherSnapSocket))
            {
                return false;
            }

            if (socketInReach != null && socketInReach.Socket.Module == otherSnapSocket.Socket.Module)
            {
                return false;
            }

            if (SnapModule.IsLocked || otherSnapSocket.SnapModule.IsLocked)
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
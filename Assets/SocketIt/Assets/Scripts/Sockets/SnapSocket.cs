using System;
using System.Collections.Generic;
using UnityEngine;

namespace SocketIt
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Socket))]
    [AddComponentMenu("SocketIt/Socket/Snap Socket")]
    public class SnapSocket : MonoBehaviour
    {
        public delegate void SnapSocketEvent(Snap snap);
        public event SnapSocketEvent OnSnap;

        private SnapModule snapModule;

        private Socket socket;

        private Module module;

        [Range(0.0f, 180.0f)]
        public float AngleLimit = 180;

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

        public SnapModule SnapModule
        {
            get
            {
                return snapModule;
            }
        }

        public void Reset()
        {
            if (GetComponent<Socket>().Module.GetComponent<SnapModule>() == null)
            {
                GetComponent<Socket>().Module.gameObject.AddComponent<SnapModule>();
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

            snapModule = socket.Module.GetComponent<SnapModule>();

            if (snapModule == null)
            {
                throw new SocketException("SnapSocket is not connected to a SnapModule");
            }
        }

        public void Snap(SnapSocket OtherSnapSocket)
        {
            if (!isValidSocket(OtherSnapSocket))
            {
                return;
            }

            if (OnSnap != null)
            {
                OnSnap(new Snap(this, OtherSnapSocket));
            }

            snapModule.OnSocketSnap(this, OtherSnapSocket);
        }

        private bool isValidSocket(SnapSocket otherSnapSocket)
        {
            if (otherSnapSocket == this)
            {
                return false;
            }

            if (socket.Module == otherSnapSocket.Socket.Module)
            {
                return false;
            }

            if (Socket.GetConnectedSocket() != null || otherSnapSocket.Socket.GetConnectedSocket() != null)
            {
                return false;
            }

            if (AngleLimit < 180 && !isInsideAngle(otherSnapSocket))
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
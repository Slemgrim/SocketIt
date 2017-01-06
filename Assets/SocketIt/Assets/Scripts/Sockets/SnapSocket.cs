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

        public SnapModule SnapModule;

        public Socket Socket;

        public Module Module;

        [Range(0.0f, 180.0f)]
        public float AngleLimit = 180;

        public void Reset()
        {
            if (GetComponent<Socket>().Module.GetComponent<SnapModule>() == null)
            {
                GetComponent<Socket>().Module.gameObject.AddComponent<SnapModule>();
            }

            Socket = GetComponent<Socket>();
            Module = Socket.Module;
            SnapModule = Socket.Module.GetComponent<SnapModule>();
        }

        void OnDrawGizmos()
        {
            SocketItGizmo.DrawSocketAngle(this, AngleLimit);
        }

        public void Awake()
        {
            if(Socket == null)
            {
                throw new SocketException("SnapSocket needs a Socket component");
            }

            if (Module == null)
            {
                throw new SocketException("SnapSocket needs to a SnapModule component");
            }

            if (SnapModule == null)
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

            SnapModule.OnSocketSnap(this, OtherSnapSocket);
        }

        private bool isValidSocket(SnapSocket otherSnapSocket)
        {
            if (otherSnapSocket == this)
            {
                return false;
            }

            if (Socket.Module == otherSnapSocket.Socket.Module)
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
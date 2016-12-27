using UnityEngine;
using System.Collections;
using System;
using SocketIt.Examples;

namespace SocketIt.Example00
{
    public class BodyConnector : MonoBehaviour, IModuleConnector
    {
        public MouseControll mouseControll;

        public event ModuleConnectEvent OnConnectEnd;
        public event ModuleConnectEvent OnConnectStart;
        public event ModuleConnectEvent OnDisconnectEnd;
        public event ModuleConnectEvent OnDisconnectStart;

        public void Awake()
        {
            Module module = GetComponent<Module>();
            ISocketSnapper snapper = GetComponent<ISocketSnapper>();

            module.OnConnect += OnConnect;
            snapper.OnSnapEnd += OnSnap;
            mouseControll.OnPickUp += OnPickUp;
        }

        private void OnPickUp(GameObject follower)
        {
            follower.GetComponent<Module>().DisconnectAll();
        }

        private void OnSnap(Snap snap)
        {
            snap.SocketA.Socket.Connect(snap.SocketB.Socket);   
        }

        private void OnConnect(Connection connection)
        {
            HingeJoint joint = gameObject.AddComponent<HingeJoint>();

            JointLimits limits = joint.limits;
            limits.min = 0;
            limits.bounciness = 0;
            limits.bounceMinVelocity = 0;
            limits.max = 90;
            joint.limits = limits;
            joint.useLimits = true;

            joint.connectedBody = connection.SocketB.Module.GetComponent<Rigidbody>();
        }
    }

}

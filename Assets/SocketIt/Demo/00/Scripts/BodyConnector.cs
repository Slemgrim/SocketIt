using UnityEngine;
using System.Collections;
using System;
using SocketIt.Examples;
using System.Collections.Generic;

namespace SocketIt.Example00
{
    public class BodyConnector : MonoBehaviour
    {
        public MouseControll mouseControll;

        private Module module;

        public void Awake()
        {
            module = GetComponent<Module>();
            mouseControll.OnDropOff += OnDrop;
            module.OnConnect += OnConnect;
            module.OnDisconnect += OnDisconnect;

            mouseControll.OnPickUp += OnPickUp;
        }

        private void OnDrop(GameObject follower)
        {
            if(follower == gameObject && mouseControll.CurrentSnap != null)
            {
                mouseControll.CurrentSnap.SocketA.Socket.Connect(mouseControll.CurrentSnap.SocketB.Socket);
            }
        }

        private void OnPickUp(GameObject follower)
        {
            if (follower.transform.parent == null)
            {
                return;
            }

            Module parent = follower.transform.parent.GetComponent<Module>();

            parent.DisconnectModule(follower.GetComponent<Module>());
            follower.transform.SetParent(null);
        }

        private void OnConnect(Connection connection)
        {
            if(connection.Initiator.Module != module)
            {
                return;
            }

            CharacterJoint joint = gameObject.AddComponent<CharacterJoint>();

            joint.connectedBody = connection.SocketB.Module.GetComponent<Rigidbody>();
            joint.anchor = connection.SocketA.transform.localPosition;

            connection.SocketA.Module.transform.SetParent(connection.SocketB.Module.transform);
            
        }

        private void OnDisconnect(Connection connection)
        {
            List<CharacterJoint> joints = new List<CharacterJoint>(GetComponents<CharacterJoint>());
            foreach (CharacterJoint joint in joints)
            {
                if (joint.connectedBody == connection.SocketB.Module.GetComponent<Rigidbody>())
                {
                    Destroy(joint);
                }
            }

            connection.SocketA.Module.transform.SetParent(null);
        }
    }
}

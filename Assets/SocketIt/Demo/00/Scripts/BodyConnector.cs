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

        private Module Module;

        public void Awake()
        {
            Module = GetComponent<Module>();
            mouseControll.OnDropOff += OnDrop;
            Module.OnModuleConnected += OnConnect;
            Module.OnModuleDisconnected += OnDisconnect;

            mouseControll.OnPickUp += OnPickUp;
        }

        private void OnDrop(GameObject follower)
        {
            if(follower == gameObject && mouseControll.CurrentSnap != null)
            {
                mouseControll.CurrentSnap.SocketB.Connect(mouseControll.CurrentSnap.SocketA);
            }
        }

        private void OnPickUp(GameObject follower)
        {
            if (follower.transform.parent == null)
            {
                return;
            }

            Module disconnector = follower.transform.parent.GetComponent<Module>();
            Module disconectee = follower.GetComponent<Module>();

            disconnector.DisconnectModule(disconectee);
            follower.transform.SetParent(null);
        }

        private void OnConnect(Connection connection)
        {
            if (connection.Connectee.Module != Module)
            {
                return;
            }
            
            CharacterJoint joint = gameObject.AddComponent<CharacterJoint>();

            joint.connectedBody = connection.Connector.Module.GetComponent<Rigidbody>();
            joint.anchor = connection.Connectee.transform.localPosition;
            
            connection.Connectee.Module.transform.SetParent(connection.Connector.Module.transform);
            
        }

        private void OnDisconnect(Connection connection)
        {
            List<CharacterJoint> joints = new List<CharacterJoint>(GetComponents<CharacterJoint>());

            foreach (CharacterJoint joint in joints)
            {
                if (joint.connectedBody == connection.Connector.Module.GetComponent<Rigidbody>())
                {
                    Destroy(joint);
                }
            }

            if (connection.Connectee.Module.Composition == null)
            {
                connection.Connector.Module.transform.SetParent(null);
            }
        }
    }
}

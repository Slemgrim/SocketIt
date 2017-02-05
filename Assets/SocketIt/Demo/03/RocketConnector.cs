using UnityEngine;
using System.Collections;
using System;
using SocketIt.Examples;
using System.Collections.Generic;

namespace SocketIt.Example00
{
    public class RocketConnector : MonoBehaviour
    {
        public MouseControll mouseControll;

        private Module Module;

        public void Awake()
        {
            Module = GetComponent<Module>();
            mouseControll.OnDropOff += OnDrop;
            mouseControll.OnPickUp += OnPickUp;
        }

        private void OnDrop(GameObject follower)
        {
            if (follower == gameObject && mouseControll.CurrentSnap != null)
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

        public void OnConnect(Connection connection)
        {
            if (connection.Connectee.Module != Module)
            {
                return;
            }

            connection.Connectee.Module.transform.SetParent(connection.Connector.Module.transform);
        }

        public void OnDisconnect(Connection connection)
        {
            if (connection.Connectee.Module.Composition == null)
            {
                connection.Connector.Module.transform.SetParent(null);
            }
        }
    }
}

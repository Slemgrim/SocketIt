﻿using UnityEngine;
using SocketIt.Examples;
using System;

namespace SocketIt.Examples
{
    public class NodeControll : MonoBehaviour
    {
        private MouseControll mouseControll;

        private Snap snap = null;

        void Awake()
        {
            mouseControll = GetComponent<MouseControll>();
            mouseControll.OnPickUp += OnPickUp;
            mouseControll.OnDropOff += OnDropOff;
            mouseControll.OnSnapStart += OnSnapStart;
            mouseControll.OnSnapEnd += OnSnapEnd;
        }

        private void OnSnapStart(Snap snap)
        {
            this.snap = snap;
        }

        private void OnSnapEnd(Snap snap)
        {
            this.snap = null;
        }

        private void OnDropOff(GameObject follower)
        {
            if(snap != null)
            {
                Connect(snap);
            }

            snap = null;
        }

        private void Connect(Snap snap)
        {
            snap.SocketA.Socket.Connect(snap.SocketB.Socket);
            snap.SocketA.Module.transform.SetParent(snap.SocketB.Module.transform);
        }

        private void OnPickUp(GameObject follower)
        {
            if (follower.transform.parent == null)
            {
                return;
            }

            Module parent = follower.transform.parent.GetComponent<Module>();

            parent.DisconnectModule(follower.GetComponent<Module>());

            //ClearParentConnection(follower);
            follower.transform.SetParent(null);

            snap = null;
        }

        private void ClearParentConnection(GameObject follower)
        {
            follower.transform.parent = null;
            NodeModule node = follower.GetComponent<NodeModule>();

            if(node.ParentNode == null)
            {
                return;
            }

            Connection connection = node.Module.GetConnection(node.ParentNode.Module);

            if (connection != null)
            {
                connection.SocketA.Disconnect(connection.SocketB);
            }
        }
    }
}



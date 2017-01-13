using UnityEngine;
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
            snap.SocketA.Connect(snap.SocketB);
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
            follower.transform.SetParent(null);

            snap = null;
        }
    }
}



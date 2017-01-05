using UnityEngine;
using System.Collections;
using SocketIt.Examples;
using System;
using System.Collections.Generic;

namespace SocketIt.Example05
{
    public class RayController : MonoBehaviour
    {
        private MouseControll mouseControll;

        void Awake()
        {
            mouseControll = GetComponent<MouseControll>();
            mouseControll.OnPickUp += OnPickUp;
            mouseControll.OnDropOff += OnDropOff;
            mouseControll.OnSnapEnd += OnSnapEnd;

        }

        private void OnPickUp(GameObject follower)
        {
            ClearSockets(follower);
            SetRaysActive(follower, true);
        }

        private void OnDropOff(GameObject follower)
        {
            SetRaysActive(follower, false);
        }

        private void OnSnapEnd(Snap snap)
        {
            ClearSockets(snap.SocketA.Module.gameObject);
        }

        private void ClearSockets(GameObject follower)
        {
            List<RaySocket> raySockets = new List<RaySocket>(follower.GetComponentsInChildren<RaySocket>());
            foreach (RaySocket raySocket in raySockets)
            {
                raySocket.Clear();
            }
        }

        private void SetRaysActive(GameObject follower, bool status)
        {
            List<RaySocket> raySockets = new List<RaySocket>(follower.GetComponentsInChildren<RaySocket>());
            foreach (RaySocket raySocket in raySockets)
            {
                raySocket.RaysActive = status;
            }
        }
    }
}



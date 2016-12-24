using UnityEngine;
using System.Collections;
using System;

namespace SocketIt
{
    public class LerpSnapper : MonoBehaviour, ISocketSnapper
    {
        private Quaternion targetRotation;
        private Vector3 targetPosition;

        public event SnapEvent OnSnapEnd;
        public event SnapEvent OnSnapStart;

        private bool IsSnapping = false;
        private Snap snap;

        private SnapModule module;

        public void Start()
        {
            module = GetComponent<SnapModule>();

            if (module == null)
            {
                throw new SnapperException("Snapper needs a snap module attached");
            }

            module.OnSnap += StartSnapping;
        }

        public void StartSnapping(Snap snap)
        {
            Debug.Log("Start Lerping");

            targetRotation = GetTargetRotation(snap.SocketA, snap.SocketB);
            targetPosition = GetTargetPosition(snap.SocketA, snap.SocketB);


            this.snap = snap;
            IsSnapping = true;

            if(OnSnapStart != null)
            {
                OnSnapStart(snap);
            }
        }

        public void StopSnapping()
        {
            targetRotation = Quaternion.identity;
            targetPosition = Vector3.zero;

            Debug.Log("Stop Lerping");


            IsSnapping = false;

            if (OnSnapEnd != null)
            {
                OnSnapEnd(snap);
            }

            this.snap = null;
        }

        public Quaternion GetTargetRotation(SnapSocket ownSocket, SnapSocket otherSocket)
        {
            Quaternion diff = Quaternion.Inverse(otherSocket.transform.rotation) * ownSocket.transform.rotation;

            ownSocket.Module.transform.rotation = diff * ownSocket.Module.transform.rotation;


            return diff;
        }

        public Vector3 GetTargetPosition(SnapSocket ownSocket, SnapSocket otherSocket)
        {
            Vector3 ownSocketPosition = ownSocket.transform.localPosition;
            ownSocketPosition = ownSocket.Module.transform.rotation * ownSocketPosition;
            //ownSocket.Module.transform.position = otherSocket.transform.position - ownSocketPosition;

            return Vector3.zero;
        }

        void Update()
        {
            if (!IsSnapping)
            {
                return;
            }

            //snap.SocketA.Module.transform.rotation = Quaternion.Slerp(snap.SocketA.Module.transform.rotation, targetRotation, Time.time * 0.05f);

            if(snap.SocketA.Module.transform.rotation == targetRotation)
            {
                StopSnapping();
            }
        }
    }
}

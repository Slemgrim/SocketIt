using UnityEngine;
using System.Collections;
using SocketIt;
using System;

namespace SocketIt.Demo
{
    public class SnapHelper : MonoBehaviour
    {
        public SnapSocket snapSocketA;
        public SnapSocket snapSocketB;

        private Vector3 initPosition;
        private Quaternion initRotation;

        private bool isSnapped = false;

        void Start()
        {
            initPosition = snapSocketA.Module.transform.position;
            initRotation = snapSocketA.Module.transform.rotation;


            ISocketSnapper snapper = snapSocketA.Module.GetComponent<ISocketSnapper>();

            snapper.OnSnapStart += OnSnapStart;
            snapper.OnSnapEnd += OnSnapEnd;
        }

        private void OnSnapEnd(Snap snap)
        {
            Debug.Log(string.Format(
                   "Beginn snapping {0}.{1} to {2}.{3}",
                   snap.SocketA.Module.name,
                   snap.SocketA.name,
                   snap.SocketB.Module.name,
                   snap.SocketB.name
            ));
        }

        private void OnSnapStart(Snap snap)
        {
            Debug.Log(string.Format(
                "End snapping {0}.{1} to {2}.{3}",
                   snap.SocketA.Module.name,
                   snap.SocketA.name,
                   snap.SocketB.Module.name,
                   snap.SocketB.name
            ));
        }

        void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                if (isSnapped)
                {
                    //snapSocketA.Lost(snapSocketB);
                    snapSocketA.Module.transform.parent = null;

                    snapSocketA.Module.transform.position = initPosition;
                    snapSocketA.Module.transform.rotation = initRotation;
                    isSnapped = false;
                } else
                {
                    snapSocketA.Snap(snapSocketB);
                    isSnapped = true;
                }
            }
        }
    }
}

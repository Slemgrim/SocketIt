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

        void Start()
        {
            initPosition = snapSocketA.Module.transform.position;
            initRotation = snapSocketA.Module.transform.rotation;

            ISocketSnapper snapperB = snapSocketB.Module.GetComponent<ISocketSnapper>();

            snapperB.OnSnapStart += OnSnapStart;
            snapperB.OnSnapEnd += OnSnapEnd;
        }

        private void OnSnapEnd(SnapSocket ownSocket, SnapSocket otherSocket)
        {
            Debug.Log(string.Format(
                   "Beginn snapping {0}.{1} to {2}.{3}",
                   ownSocket.Module.name,
                   ownSocket.name,
                   otherSocket.Module.name,
                   otherSocket.name
            ));
        }

        private void OnSnapStart(SnapSocket ownSocket, SnapSocket otherSocket)
        {
            Debug.Log(string.Format(
                "End snapping {0}.{1} to {2}.{3}",
                ownSocket.Module.name,
                ownSocket.name,
                otherSocket.Module.name,
                otherSocket.name
            ));
        }

        void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                snapSocketA.Found(snapSocketB);
            }

            if (Input.GetKeyDown("r"))
            {
                snapSocketA.Lost(snapSocketB);

                snapSocketA.Module.transform.position = initPosition;
                snapSocketA.Module.transform.rotation = initRotation;
            }
        }
    }
}

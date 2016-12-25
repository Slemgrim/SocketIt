using UnityEngine;
using System.Collections;
using System;
using SocketIt;
using System.Collections.Generic;
using SocketIt.Examples;

namespace SocketIt.Examples
{
    public class MouseMoveable : MonoBehaviour
    {
        private GameObject mouseFollower;
        private SnapModule snapModule;
        private ISocketSnapper snapper;
        public float snapDistance = 1f;
        public bool connectOnClick = false;

        private bool isSnapped = false;

        private Snap snap;

        private Vector3 mousePosition;

        public void Update()
        {
            mousePosition = Input.mousePosition;
            mousePosition.z = transform.position.z - Camera.main.transform.position.z;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            if (Input.GetMouseButtonDown(0))
            {

                if (mouseFollower != null)
                {
                    Connect();
                    UnsetMouseFollower();
                }
                else
                {
                    SetMouseFollower();
                }
            }

            if (mouseFollower != null)
            {
                Follow(mouseFollower);
            }

        }

        private void Connect()
        {
            if (connectOnClick && snap != null)
            {
                Debug.Log("Connect");
                snap.SocketA.GetComponent<Socket>().Connect(snap.SocketB.GetComponent<Socket>());
            }
        }

        private void Follow(GameObject mouseFollower)
        {
            if (isSnapped && Vector3.Distance(mouseFollower.transform.position, mousePosition) > snapDistance)
            {
                isSnapped = false;
                List<RaySocket> raySockets = new List<RaySocket>(mouseFollower.GetComponentsInChildren<RaySocket>());
                foreach (RaySocket raySocket in raySockets)
                {
                    raySocket.Reset();
                }

                List<SnapSocket> snapSockets = new List<SnapSocket>(mouseFollower.GetComponentsInChildren<SnapSocket>());
                foreach (SnapSocket snapSocket in snapSockets)
                {
                    snapSocket.Clear();
                }
            }

            if (isSnapped)
            {
                return;
            }

            mouseFollower.transform.position = mousePosition;

        }

        private void SetMouseFollower()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject follower =  hit.collider.gameObject;
                SnapModule snapModule = follower.GetComponent<SnapModule>();
                ISocketSnapper snapper = follower.GetComponent<ISocketSnapper>();

                if (snapModule == null || snapper == null)
                {
                    return;
                }

                this.mouseFollower = follower;
                this.snapModule = snapModule;
                this.snapper = snapper;

                snapModule.IsStatic = false;

                snapper.OnSnapEnd += OnSnap;

                MakeTransparent(mouseFollower);

                List<RaySocket> raySockets = new List<RaySocket>(follower.GetComponentsInChildren<RaySocket>());
                foreach(RaySocket raySocket in raySockets)
                {
                    raySocket.RaysActive = true;
                }
            }
        }

        private void UnsetMouseFollower()
        {
            MakeOpaque(mouseFollower);

            List<RaySocket> raySockets = new List<RaySocket>(mouseFollower.GetComponentsInChildren<RaySocket>());
            foreach (RaySocket raySocket in raySockets)
            {
                raySocket.RaysActive = false;
            }

            snapModule.IsStatic = true;
            mouseFollower = null;
            snapModule = null;

            snapper.OnSnapEnd -= OnSnap;
            snapper = null;
            isSnapped = false;
            snap = null;

        }

        private void OnSnap(Snap snap)
        {
            this.snap = snap;
            isSnapped = true;
        }

        private void MakeTransparent(GameObject go)
        {
            List<TransparencyController> transparencyControllers = new List<TransparencyController>(go.GetComponentsInChildren<TransparencyController>());
            transparencyControllers.Add(go.GetComponent<TransparencyController>());
            foreach(TransparencyController controller in transparencyControllers)
            {
                controller.MakeTransparent();
            }
        }

        private void MakeOpaque(GameObject go)
        {
            List<TransparencyController> transparencyControllers = new List<TransparencyController>(go.GetComponentsInChildren<TransparencyController>());
            transparencyControllers.Add(go.GetComponent<TransparencyController>());
            foreach (TransparencyController controller in transparencyControllers)
            {
                controller.RestoreDefault();
            }
        }
    }
}

using UnityEngine;
using System.Collections;
using System;
using SocketIt;

namespace SocketIt.Demo
{
    public class MouseMoveable : MonoBehaviour
    {
        private GameObject mouseFollower;
        private SnapModule snapModule;
        private ISocketSnapper snapper;
        public float snapDistance = 1f;

        private bool isSnapped = false;

        public void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                if (mouseFollower != null)
                {
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

        private void Follow(GameObject mouseFollower)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = transform.position.z - Camera.main.transform.position.z;
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            if(isSnapped && Vector3.Distance(mouseFollower.transform.position, newPosition) > snapDistance)
            {
                isSnapped = false;
            }

            if (isSnapped)
            {
                return;
            }

            mouseFollower.transform.position = newPosition;

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
            }
        }

        private void UnsetMouseFollower()
        {
            snapModule.IsStatic = true;
            mouseFollower = null;
            snapModule = null;

            snapper.OnSnapEnd -= OnSnap;
            snapper = null;
            isSnapped = false;
        }

        private void OnSnap(SnapSocket ownSocket, SnapSocket otherSocket)
        {
            isSnapped = true;
        }
    }
}

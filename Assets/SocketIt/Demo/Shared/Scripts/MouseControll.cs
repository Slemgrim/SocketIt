using UnityEngine;
using System.Collections;
using System;
using SocketIt;
using System.Collections.Generic;
using SocketIt.Examples;

namespace SocketIt.Examples
{
    public class MouseControll : MonoBehaviour
    {
        /*
         * Distance betwenn mouse and follower until snapping stops
         */
        public float snapDistance = 1f;

        public delegate void MouseEvent(GameObject follower);
        public event MouseEvent OnPickUp;
        public event MouseEvent OnDropOff;
        public event SnapEvent OnSnapStart;
        public event SnapEvent OnSnapEnd;

        /*
         * Z distance of mouse pointer
         */
        private float mouseDistance = 0;

        /*
         * GameObject that is controlled by mouse pointer 
         */
        private GameObject follower;

        /*
         * Is the follower in snapping mode
         */
        private Snap currentSnap = null;

        /*
         * Current mouse position in world space.
         */
        private Vector3 currentMousePosition;

        public Snap CurrentSnap
        {
            get
            {
                return currentSnap;
            }
        }

        public void Update()
        {
            currentMousePosition = GetCurrentMousePositionInWorldSpace();

            if (Input.GetMouseButtonDown(0))
            {
                if (follower != null)
                {
                    UnsetMouseFollower();
                }
                else
                {
                    SetMouseFollower();
                }
            }

            Follow(follower, currentMousePosition);
        }

        private Vector3 GetCurrentMousePositionInWorldSpace()
        {
            if (mouseDistance == 0) {
                mouseDistance = transform.position.z - Camera.main.transform.position.z;
            }

            Vector3 pos = Input.mousePosition;
            pos.z = mouseDistance;
            pos = Camera.main.ScreenToWorldPoint(pos);
            return pos;
        }



        private void SetMouseFollower()
        {
            GameObject clickedObject = GetClickedObject();

            // Return if no object has been clicked
            if(clickedObject == null)
            {
                return;
            }

            SnapModule snapModule = clickedObject.GetComponent<SnapModule>();
            ISocketSnapper snapper = clickedObject.GetComponent<ISocketSnapper>();

            // Return if we don't have snap components. In a real project you would do this with layer or tag checking
            if(snapModule == null || snapper == null)
            {
                return;
            }

            follower = clickedObject;


            // Mark the snap module as moveable. Only non static objects can snap to static object. We need this to
            // prevent the snapping system from destroying static structures
            snapModule.IsStatic = false;

            // Register to the snapper OnSnapEnd delegate so we are able to stop folling while snapping
            snapper.OnSnapEnd += OnSnap;

            // Make the clicked object transparent
            MakeTransparent(this.follower);

            if (OnPickUp != null)
            {
                OnPickUp(follower);
            }
            
        }

        /**
         * Get the GameObject under the mouse curser
         */
        private GameObject GetClickedObject()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                return hit.collider.gameObject;
            }

            return null;
        }
        
        private void UnsetMouseFollower()
        {
            // Remove all transparency
            MakeOpaque(follower);

            SnapModule snapModule = follower.GetComponent<SnapModule>();
            ISocketSnapper snapper = follower.GetComponent<ISocketSnapper>();

            // Return if we don't have snap components. In a real project you would do this with layer or tag checking
            if (snapModule == null || snapper == null)
            {
                return;
            }

            //Restore the module to static
            snapModule.IsStatic = true;

            if (OnDropOff != null)
            {
                OnDropOff(follower);
            }

            //Remove the reference to the follower;
            follower = null;
            snapper.OnSnapEnd -= OnSnap;

            //Reset the isSnapped property to avoid errors with the next followed module
            currentSnap = null;
        }

        private void OnSnap(Snap snap)
        {
            //Mark the object as snapped so we can stop moving it around
            currentSnap = snap;

            //Reset mouse distance so we can make better calculations about distance between mouse and snapped object
            mouseDistance = snap.SocketB.transform.position.z - Camera.main.transform.position.z;

            if(OnSnapStart != null)
            {
                OnSnapStart(snap);
            }
        }

        /**
         * Updates the follower position with the given position
         */
        private void Follow(GameObject follower, Vector3 newPosition)
        {
            if (follower == null)
            {
                return;
            }

            //Stop snapping if the new position is to far away from a snaped follwer
            if (currentSnap != null && Vector3.Distance(follower.transform.position, newPosition) > snapDistance)
            {
                if (OnSnapEnd != null)
                {
                    OnSnapEnd(currentSnap);
                }

                currentSnap = null;
            }

            //Do not update the position if the object is in snapping mode
            if (currentSnap != null)
            {
                return;
            }

            follower.transform.position = newPosition;
        }

        /**
         * Make the game object and its children transparent. We do this to visualize a user controlled gameobject
         */
        private void MakeTransparent(GameObject go)
        {
            List<TransparencyController> transparencyControllers = new List<TransparencyController>(go.GetComponentsInChildren<TransparencyController>());
            TransparencyController transparencyController = go.GetComponent<TransparencyController>();

            if (transparencyController != null)
            {
                transparencyControllers.Add(transparencyController);
            }
            foreach (TransparencyController controller in transparencyControllers)
            {
                controller.MakeTransparent();
            }
        }

        /**
         * Remove transparency from gameobject and its children
         */
        private void MakeOpaque(GameObject go)
        {
            List<TransparencyController> transparencyControllers = new List<TransparencyController>(go.GetComponentsInChildren<TransparencyController>());
            TransparencyController transparencyController = go.GetComponent<TransparencyController>();

            if (transparencyController != null)
            {
                transparencyControllers.Add(transparencyController);
            }

            foreach (TransparencyController controller in transparencyControllers)
            {
                controller.RestoreDefault();
            }
        }
    }
}

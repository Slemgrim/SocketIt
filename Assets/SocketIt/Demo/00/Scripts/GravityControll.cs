using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SocketIt.Example00
{
    public class GravityControll : MonoBehaviour
    {
        public List<GameObject> parts = new List<GameObject>();

        private bool hasGravity = false;

        void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                if (hasGravity)
                {
                    SetGravity(false);
                }
                else
                {
                    SetGravity(true);
                }
            }
        }

        private void SetGravity(bool state)
        {
            foreach(GameObject part in parts)
            {
                Rigidbody rb = part.GetComponent<Rigidbody>();
                if(state == false)
                {
                    rb.useGravity = false;
                    rb.isKinematic = true;
                } else
                {
                    rb.useGravity = true;
                    rb.isKinematic = false;
                }
            }

            hasGravity = state;
        }
    }
}
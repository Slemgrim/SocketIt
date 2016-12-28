using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIt.Examples;

namespace SocketIt.Example00
{
    public class GravityControll : MonoBehaviour
    {
        public List<GameObject> parts = new List<GameObject>();
        private List<Quaternion> rotations = new List<Quaternion>();
        private List<Vector3> positions = new List<Vector3>();

        private MouseControll mouseControll;

        private bool hasGravity = false;

        void Start()
        {
            mouseControll = GetComponent<MouseControll>();
        }

        void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                if (hasGravity)
                {
                    UnsetGravity();
                    mouseControll.Enable();
                }
                else
                {
                    mouseControll.Disable();
                    SetGravity();
                }
            }
        }

        private void SetGravity()
        {

            positions.Clear();
            rotations.Clear(); foreach (GameObject part in parts)
            {
                positions.Add(part.transform.localPosition);
                rotations.Add(part.transform.localRotation);

                Rigidbody rb = part.GetComponent<Rigidbody>();
                rb.useGravity = true;
                rb.isKinematic = false;
            }

            hasGravity = true;
        }

        private void UnsetGravity()
        {
            for (int i = 0; i<parts.Count; i++)
            {
                GameObject part = parts[i];
                part.transform.localPosition = positions[i];
                part.transform.localRotation = rotations[i];

                Rigidbody rb = part.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            positions.Clear();
            rotations.Clear();

            hasGravity = false;
        }
    }
}
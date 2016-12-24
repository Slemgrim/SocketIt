using UnityEngine;
using System.Collections;
using System;

namespace SocketIt
{

    public class RotateSnapper : MonoBehaviour, ISocketSnapper
    {
        public event SnapEvent OnSnapStart;
        public event SnapEvent OnSnapEnd;

        /**
		 * Does this Socket snap its Position to the other Socket;
		 */
        public bool SnapPosition = true;

        /**
		 * Does this Socket snap its forward rotation to the other Socket;
		 */
        public bool SnapRotationForward = true;

        /**
		 * Does this Socket snap its up rotation to the other Socket;
		 */
        public bool SnapRotationUp = true;

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
            SnapSocket ownSocket = snap.SocketA;
            SnapSocket otherSocket = snap.SocketB;

            if (OnSnapStart != null)
            {
                OnSnapStart(new Snap(ownSocket, otherSocket));
            }

            ownSocket.Lock();
            otherSocket.Lock();

            RotateToOtherSocket(ownSocket, otherSocket);
            MoveToOtherSocket(ownSocket, otherSocket);

            Debug.Log("Start Snapping");

            transform.parent = otherSocket.Module.transform.parent;

            ownSocket.Unlock();
            otherSocket.Unlock();

            if (OnSnapEnd != null)
            {
                OnSnapEnd(new Snap(ownSocket, otherSocket));
            }
        }

        private void RotateToOtherSocket(SnapSocket ownSocket, SnapSocket otherSocket)
        {
            if (SnapRotationForward)
            {
                Quaternion forwardRot = Quaternion.FromToRotation(
                    ownSocket.transform.forward,
                    -otherSocket.transform.forward
                );

                ownSocket.Module.transform.rotation = forwardRot * ownSocket.Module.transform.rotation;
            }

            if (SnapRotationUp)
            {
                Quaternion upRot = Quaternion.FromToRotation(
                    ownSocket.transform.up,
                    -otherSocket.transform.up
                );

                ownSocket.Module.transform.rotation = upRot * ownSocket.Module.transform.rotation;
            }
        }

        private void MoveToOtherSocket(SnapSocket ownSocket, SnapSocket otherSocket)
        {
            if (SnapPosition)
            {
                Vector3 ownSocketPosition = ownSocket.transform.localPosition;
                ownSocketPosition = ownSocket.Module.transform.rotation * ownSocketPosition;
                ownSocket.Module.transform.position = otherSocket.transform.position - ownSocketPosition;
            }
        }

        public void StopSnapping()
        {
            //transform.parent = null;

            return;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocketIt
{
    [System.Serializable]
    public class Snap
    {
        public Socket SocketA;
        public Socket SocketB;

        public Snap(Socket socketA, Socket socketB)
        {
            SocketA = socketA;
            SocketB = socketB;
        }

        public SnapTransform GetTargetTransform()
        {
            return GetTargetTransform(true, true, true);
        }

        public SnapTransform GetTargetTransform(bool snapPosition)
        {
            return GetTargetTransform(snapPosition, true, true);
        }

        public SnapTransform GetTargetTransform(bool snapPosition, bool snapForward)
        {
            return GetTargetTransform(snapPosition, snapForward, true);
        }

        public SnapTransform GetTargetTransform(bool snapPosition, bool snapForward, bool snapUp)
        { 
            Vector3 originalPosition = SocketA.Module.transform.position;
            Quaternion originalRotation = SocketA.Module.transform.rotation;

            if (snapForward)
            {
                SnapForward();
            }

            if (snapUp)
            {
                SnapUp();
            }

            if (snapPosition)
            {
                SnapPosition();
            }

            SnapTransform tempTranform = new SnapTransform();
            tempTranform.position = SocketA.Module.transform.position;
            tempTranform.rotation = SocketA.Module.transform.rotation;

            SocketA.Module.transform.position = originalPosition;
            SocketA.Module.transform.rotation = originalRotation;

            return tempTranform;
        }

        private void SnapForward()
        {
            Quaternion forwardRot = Quaternion.FromToRotation(
                SocketA.transform.forward,
                -SocketB.transform.forward
            );

            SocketA.Module.transform.rotation = forwardRot * SocketA.Module.transform.rotation;
        }

        private void SnapUp()
        {
            Quaternion upRot = Quaternion.FromToRotation(
                SocketA.transform.up,
                -SocketB.transform.up
            );

            SocketA.Module.transform.rotation = upRot * SocketA.Module.transform.rotation;
        }

        private void SnapPosition()
        {
            Vector3 ownSocketPosition = SocketA.transform.localPosition;
            ownSocketPosition = SocketA.Module.transform.rotation * ownSocketPosition;
            SocketA.Module.transform.position = SocketB.transform.position - ownSocketPosition;
        }
    }
}

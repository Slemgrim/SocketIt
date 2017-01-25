using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

namespace SocketIt {
    [DisallowMultipleComponent]
    [AddComponentMenu("SocketIt/Snapper/Instant Snapper")]
    public class InstantSnapper : MonoBehaviour
	{
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

        [System.Serializable]
        public class SnapEvent : UnityEvent<Snap> { }
        public SnapEvent OnSnapStart;
        public SnapEvent OnSnapEnd;

        public void StartSnapping(Snap snap)
        {
            OnSnapStart.Invoke(new Snap(snap.SocketA, snap.SocketB));

            Snap currentSnap = snap;
            SnapTransform targetTransform = snap.GetTargetTransform(SnapPosition, SnapRotationForward, SnapRotationUp);

            snap.SocketA.Module.transform.position = targetTransform.position;
            snap.SocketA.Module.transform.rotation = targetTransform.rotation;

            OnSnapEnd.Invoke(currentSnap);

            currentSnap = null;
            targetTransform = null;
        }

        public void StopSnapping()
        {
            return;
        }
    }
}
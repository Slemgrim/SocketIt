using UnityEngine;
using System.Collections;
using System;

namespace SocketIt
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Module))]
    [AddComponentMenu("SocketIt/Snapper/Interpolated Snapper")]
    public class InterpolatedSnapper : MonoBehaviour, ISocketSnapper
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

        public AnimationCurve easing = AnimationCurve.Linear(0, 0, 1, 1);

        private bool IsSnapping = false;

        public float SnapDuration = 1;
        private float Progress = 0;
        private Snap currentSnap = null;

        private Module module;

        private SnapTransform targetTransform = null;
        private SnapTransform startTransform = null;

        public void Start()
        {
            module = GetComponent<Module>();

            if (module == null)
            {
                throw new SnapperException("Snapper needs a snap module attached");
            }

            module.OnSnap += StartSnapping;
        }

        public void StartSnapping(Snap snap)
        {
            if (IsSnapping)
            {
                return;
            }

            if (OnSnapStart != null)
            {
                OnSnapStart(new Snap(snap.SocketA, snap.SocketB));
            }

            currentSnap = snap;
            targetTransform = snap.GetTargetTransform(SnapPosition, SnapRotationForward, SnapRotationUp);

            SnapOverTime(snap);
        }

        private void EndSnap(Snap snap)
        {
            snap.SocketA.Module.transform.position = targetTransform.position;
            snap.SocketA.Module.transform.rotation = targetTransform.rotation;

            if (OnSnapEnd != null)
            {
                OnSnapEnd(currentSnap);
            }

            ResetSnap();
        }

        private void SnapOverTime(Snap snap)
        {
            IsSnapping = true;
            startTransform = new SnapTransform();
            startTransform.position = snap.SocketA.Module.transform.position;
            startTransform.rotation = snap.SocketA.Module.transform.rotation;

            Progress = 0;
        }

        public void Update()
        {
            if (!IsSnapping)
            {
                return;
            }

            if (Progress >= SnapDuration)
            {
                EndSnap(currentSnap);
                return;
            }

            if (Progress > SnapDuration)
            {
                Progress = SnapDuration;
            }

            float t = (((Progress - 0) * (1 - 0)) / (SnapDuration - 0)) + 0;
            t = easing.Evaluate(t);
            Progress += Time.deltaTime;

            currentSnap.SocketA.Module.transform.position = Vector3.Lerp(startTransform.position, targetTransform.position, t);
            currentSnap.SocketA.Module.transform.rotation = Quaternion.Slerp(startTransform.rotation, targetTransform.rotation, t);
        }

        public void StopSnapping()
        {
            return;
        }

        private void ResetSnap()
        {
            IsSnapping = false;
            Progress = 0;
            currentSnap = null;
            targetTransform = null;
            startTransform = null;
        }
    }
}
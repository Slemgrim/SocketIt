using UnityEngine;
using System.Collections;
using System;

namespace SocketIt
{

    /// <summary>
    /// A example implementation for Snapping. InterpolatedSnapper receives a Snap and resolves 
    /// it over time without considering physics.
    /// Pay attention, Unlike InstantSnapper, InterpolatedSnapper can only execute one Snap at a time.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Module))]
    [AddComponentMenu("SocketIt/Snapper/Interpolated Snapper")]
    public class InterpolatedSnapper : MonoBehaviour
    {

        /// <summary>
        /// Apply target position of the Snap when snapping
        /// </summary>
        public bool SnapPosition = true;

        /// <summary>
        /// Apply target forward rotation of the Snap when snapping
        /// </summary>
        public bool SnapRotationForward = true;

        /// <summary>
        /// Apply target up rotation of the Snap when snapping
        /// </summary>
        public bool SnapRotationUp = true;

        /// <summary>
        /// Duration of the snap.
        /// <seealso cref="InterpolatedSnapper.Easing">
        /// </summary>
        public float SnapDuration = 1;

        /// <summary>
        /// AnimationCurve for the Snap over time.
        /// </summary>
        public AnimationCurve Easing = AnimationCurve.Linear(0, 0, 1, 1);

        /// <summary>
        /// Custom UnityEvent to listent to events from this Snapper
        /// </summary>
        public delegate void SnapEvent(Snap snap);

        /// <summary>
        /// This event fires everytime a Snap starts. 
        /// Use this to disable every other movement caused by other scripts while a Snap is happening
        /// </summary>
        public SnapEvent OnSnapStart;

        /// <summary>
        /// This event fires everytime a Snap ends. 
        /// Use this to get notified when a snap has finished. Usefull for snapping over time
        /// </summary>
        public SnapEvent OnSnapEnd;

        /// <summary>
        /// Is this InterpolatedSnapper instance currently executing a Snap?
        /// </summary>
        private bool IsSnapping = false;

        /// <summary>
        /// Progress of the current snape. This will be set to 0 after a snap endet.
        /// </summary>
        private float Progress = 0;

        /// <summary>
        /// The Snap instance which gets executed at the moment. null when no Snap is in execution.
        /// </summary>
        private Snap currentSnap = null;

        /// <summary>
        /// The calculated target of the Snap based on 
        /// InterpolatedSnapper.SnapPosition, InterpolatedSnapper.SnapRotationUp, InterpolatedSnapper.SnapRotationForward. 
        /// targetTransform only gets calculated at the beginning of a snap and doesn't update when the target Module is moving. 
        /// </summary>
        private SnapTransform targetTransform = null;

        /// <summary>
        /// The calculated start transform position and rotation of a Snap. We use this to create a smooth 
        /// interpolation between start and end posistion/rotation.
        /// </summary>
        private SnapTransform startTransform = null;

        /// <summary>
        /// Call this method to excecute a Snap
        /// </summary>
        /// <remarks>
        /// Fires InstantSnapper.OnSnapStart event right after this method gets called.
        /// </remarks>
        /// <param name="snap">The snap which should be executed</param>
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

        /// <summary>
        /// Called by Unity every frame. Updates Progress of the Snap in execution. Does nothing when no Snap is active.
        /// </summary>
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
            t = Easing.Evaluate(t);
            Progress += Time.deltaTime;

            currentSnap.SocketA.Module.transform.position = Vector3.Lerp(startTransform.position, targetTransform.position, t);
            currentSnap.SocketA.Module.transform.rotation = Quaternion.Slerp(startTransform.rotation, targetTransform.rotation, t);
        }

        /**
         *  Stops the active Snap. 
         */
        public void StopSnapping()
        {
            ResetSnap();
            return;
        }

        /// <summary>
        /// Gets called when a snap has finished and resets this instance.
        /// </summary>
        /// <remarks>
        /// Fires InstantSnapper.OnSnapEnd event after the Snap was exceuted (after InterploatedSnapper.SnapDuration seconds).
        /// </remarks>
        /// <seealso cref="StopSnapping"/>
        /// <param name="snap">The snap which should be executed</param>
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

        /// <summary>
        /// Starts a Snap and calculates the start position.
        /// </summary>
        /// <param name="snap">The snap which should be executed</param>
        private void SnapOverTime(Snap snap)
        {
            IsSnapping = true;
            startTransform = new SnapTransform();
            startTransform.position = snap.SocketA.Module.transform.position;
            startTransform.rotation = snap.SocketA.Module.transform.rotation;

            Progress = 0;
        }

        /// <summary>
        /// Resets all properties used by a Snap. We use this to reset this instance for every new Snap
        /// </summary>
        /// <seealso cref="StopSnapping"/>
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
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

namespace SocketIt {

    /// <summary>
    /// A example implementation for Snapping. InstantSnapper receives a Snap and resolves it instant without considering physics
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("SocketIt/Snapper/Instant Snapper")]
    public class InstantSnapper : MonoBehaviour
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
        /// Custom UnityEvent to listent to events from this Snapper
        /// </summary>
        [System.Serializable]
        public class SnapEvent : UnityEvent<Snap> { }

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
        /// Call this method to excecute a Snap
        /// </summary>
        /// <remarks>
        /// Fires InstantSnapper.OnSnapStart event right after this method gets called.
        /// Fires InstantSnapper.OnSnapEnd event after the Snap was exceuted.
        /// </remarks>
        /// <param name="snap">The snap which should be executed</param>
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
    }
}
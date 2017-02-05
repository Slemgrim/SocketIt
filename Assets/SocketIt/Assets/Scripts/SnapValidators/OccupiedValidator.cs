using UnityEngine;
using System.Collections;
using System;

namespace SocketIt {

    /**
     * Snap only to sockets that are not part of an existing connection
     */
    [RequireComponent(typeof(Module))]
    [AddComponentMenu("SocketIt/Snap Validators/Occupied Validator")]
    public class OccupiedValidator : MonoBehaviour, ISnapValidator
	{
        public bool SnapOnlyFreeSockets = true;

	    public bool Validate(Snap snap)
	    {
            if (SnapOnlyFreeSockets && !BothSocketsAreFree(snap))
            {
                return false;
            }

            return true;
	    }

        private bool BothSocketsAreFree(Snap snap)
        {
            return snap.SocketA.GetConnectedSocket() == null && snap.SocketB.GetConnectedSocket() == null;

        }
    }
}
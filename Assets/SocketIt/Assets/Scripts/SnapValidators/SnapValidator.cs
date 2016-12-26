using UnityEngine;
using System.Collections;
using System;

namespace SocketIt {
	public class SnapValidator : MonoBehaviour, ISnapValidator
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
            return !snap.SocketA.Socket.IsConnected && !snap.SocketB.Socket.IsConnected;

        }
    }
}
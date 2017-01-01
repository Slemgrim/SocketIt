﻿using UnityEngine;

namespace SocketIt {

	[RequireComponent(typeof(Module))]
	[AddComponentMenu("SocketIt/Connection")]
	public class Connection : MonoBehaviour
    {
        public Socket SocketA;
        public Socket SocketB;
        public Socket Initiator;

		public void OnDestroy(){
			SocketA.Disconnect (SocketB);
		}

        void OnDrawGizmosSelected()
        {
            SocketItGizmo.DrawConnection(this);
        }
    }
}

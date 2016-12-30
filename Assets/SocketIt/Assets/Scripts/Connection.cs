using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocketIt {

	[RequireComponent(typeof(Module))]
	[AddComponentMenu("SocketIt/Connection")]
	public class Connection : MonoBehaviour
    {
        public Socket SocketA;
        public Socket SocketB;
        public Socket Initiator;

		/*
		 * Sets connection state on sockets. Do not call this directly
		 */
		public void ApplyConnection()
		{
			SocketA.ConnectedSocket = SocketB;
			SocketB.ConnectedSocket = SocketA;
		}

		public void OnDestroy(){
			SocketA.Disconnect (SocketB);
		}	
    }
}

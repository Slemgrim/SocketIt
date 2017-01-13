using UnityEngine;
using System.Collections;

namespace SocketIt {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Socket))]
    [AddComponentMenu("SocketIt/Socket/Collider Socket")]
    public class ColliderSocket : MonoBehaviour {
		private Socket socket;

        void Awake(){
			socket = GetComponent<Socket> ();
		}

		void OnTriggerEnter(Collider other)
		{
            Socket otherSocket = other.GetComponent<Socket> ();

			if (otherSocket != null) {
				socket.Snap(otherSocket);
			}
		}
	}
}
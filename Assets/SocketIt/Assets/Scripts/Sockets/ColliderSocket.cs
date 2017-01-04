using UnityEngine;
using System.Collections;

namespace SocketIt {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Socket))]
    [AddComponentMenu("SocketIt/Socket/Collider Socket")]
    public class ColliderSocket : MonoBehaviour {
		private SnapSocket socket;

        void Awake(){
			socket = GetComponent<SnapSocket> ();
		}

		void OnTriggerEnter(Collider other)
		{
            SnapSocket otherSocket = other.GetComponent<SnapSocket> ();

			if (otherSocket != null) {
				socket.Snap(otherSocket);
			}
		}
	}
}
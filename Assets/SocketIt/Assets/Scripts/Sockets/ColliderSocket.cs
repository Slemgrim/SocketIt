using UnityEngine;
using System.Collections;

namespace SocketIt {
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
				socket.Found(otherSocket);
			}
		}
			
		void OnTriggerExit(Collider other)
		{
            SnapSocket otherSocket = other.GetComponent<SnapSocket> ();
		
			if (otherSocket != null) {
				socket.Lost(otherSocket);
			}
		}
	}
}
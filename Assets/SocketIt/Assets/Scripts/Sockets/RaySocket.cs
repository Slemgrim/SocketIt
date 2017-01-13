using UnityEngine;
using System.Collections;

namespace SocketIt
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Socket))]
    [AddComponentMenu("SocketIt/Socket/Ray Socket")]
    public class RaySocket : MonoBehaviour
    {

        public Camera Cam;
        public Socket Socket;

        public bool RaysActive = false;
        private RaySocket current;

        public void Reset() {
            Socket = GetComponent<Socket>();
            Cam = Camera.main;
        }

        public void Clear()
        {
            current = null;
        }

        void Update()
        {
            if (!RaysActive)
            {
                return;
            }

            if(Cam == null)
            {
                return;
            }

            Vector3 origin = Cam.transform.position;
            Vector3 direction = (transform.position - Cam.transform.position) ;

            Debug.DrawRay(origin, direction * 2, Color.green);

            Ray ray = new Ray(origin, direction);

            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);

            foreach(RaycastHit hit in hits)
            {
                if(hit.collider.gameObject == gameObject)
                {
                    continue;
                }


                RaySocket raySocket = hit.collider.GetComponent<RaySocket>();
                if(raySocket == null)
                {
                    continue;
                }

                if(current == raySocket)
                {
                    continue;
                }

                current = raySocket;
                Socket.Snap(raySocket.Socket);
            }
        }
    }
}


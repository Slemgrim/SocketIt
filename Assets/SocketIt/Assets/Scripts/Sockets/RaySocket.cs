using UnityEngine;
using System.Collections;

namespace SocketIt
{
    public class RaySocket : MonoBehaviour
    {

        public Camera camera;
        private SnapSocket socket;

        public bool RaysActive = false;
        private RaySocket current;

        void Awake()
        {
            socket = GetComponent<SnapSocket>();
        }

        public void Reset()
        {
            Debug.Log("Unset current");

            current = null;
        }

        void Update()
        {
            if (!RaysActive)
            {
                return;
            }

            if(camera == null)
            {
                return;
            }

            Vector3 origin = camera.transform.position;
            Vector3 direction = (transform.position - camera.transform.position) ;

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

                Debug.Log("Set current");

                current = raySocket;
                socket.Found(raySocket.socket);
            }
        }
    }
}


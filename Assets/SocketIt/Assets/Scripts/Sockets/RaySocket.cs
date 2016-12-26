using UnityEngine;
using System.Collections;

namespace SocketIt
{
    public class RaySocket : MonoBehaviour
    {

        public Camera cam;
        private SnapSocket socket;

        public bool RaysActive = false;
        private RaySocket current;

        void Awake()
        {
            socket = GetComponent<SnapSocket>();

            if(cam == null)
            {
                cam = Camera.main;
            }
        }

        public void Reset()
        {
            current = null;
        }

        void Update()
        {
            if (!RaysActive)
            {
                return;
            }

            if(cam == null)
            {
                return;
            }

            Vector3 origin = cam.transform.position;
            Vector3 direction = (transform.position - cam.transform.position) ;

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
                socket.Found(raySocket.socket);
            }
        }
    }
}


using UnityEngine;
using System.Collections.Generic;

namespace SocketIt.Example04
{
    /**
     * Snap two modules by clicking on their sockets
     */
    public class CickSocketController : MonoBehaviour
    {
        /**
         * List of clicked sockets.
         */
        private List<Socket> sockets = new List<Socket>();

        void Update()
        {
            Socket socket = GetClickedSocket();
            if(socket == null)
            {
                return;
            }

            AddSocket(socket);

            if (sockets.Count == 2)
            {
                SnapClickedSockets();
            }
        }

        /**
         * Get the socket under the mouse curser on click
         */
        private Socket GetClickedSocket()
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return null;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit))
            {
                return null;
            }

            Socket socket = hit.collider.GetComponent<Socket>();

            return socket;
        }

        /**
         * Add clicked socket to list of sockets
         */
        private void AddSocket(Socket socket)
        {
            if (!sockets.Contains(socket))
            {
                sockets.Add(socket);
                ChangeEmissionColor(socket.gameObject, Color.green);
            }
        }

        
        private void SnapClickedSockets()
        {
            // Snap clicked sockets.
            Snap(sockets[0], sockets[1]);

            // Empty list of clicked sockets so we can start over
            sockets.Clear();
        }

        /**
         * Snap two clicked sockets together
         */
        private void Snap(Socket firstClicked, Socket secondClicked)
        {
            firstClicked.Snap(secondClicked);
            
            //Remove emission from clicked sockets
            ChangeEmissionColor(firstClicked.gameObject, Color.black);
            ChangeEmissionColor(secondClicked.gameObject, Color.black);
        }

        private void ChangeEmissionColor(GameObject go, Color color)
        {
            Renderer renderer = go.GetComponent<Renderer>();
            Material mat = renderer.material;
            mat.SetColor("_EmissionColor", color);
        }
    }
}

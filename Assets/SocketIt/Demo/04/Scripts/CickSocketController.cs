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
        private List<SnapSocket> sockets = new List<SnapSocket>();

        void Update()
        {
            SnapSocket socket = GetClickedSocket();
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
        private SnapSocket GetClickedSocket()
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

            SnapSocket socket = hit.collider.GetComponent<SnapSocket>();

            return socket;
        }

        /**
         * Add clicked socket to list of sockets
         */
        private void AddSocket(SnapSocket socket)
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
        private void Snap(SnapSocket firstClicked, SnapSocket secondClicked)
        {

            //Make the first clicked socket moveable so we can snap it to the second one. 
            //A snap should always happen into one direction.
            firstClicked.SnapModule.IsStatic = false;
            //Snap the first clicked socket to the second one
            firstClicked.Snap(secondClicked);
            //Set first clicked socket back to static
            firstClicked.SnapModule.IsStatic = true;

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

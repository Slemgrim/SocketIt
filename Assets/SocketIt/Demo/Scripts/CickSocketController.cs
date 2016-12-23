using UnityEngine;
using System.Collections;
using SocketIt;
using System.Collections.Generic;
using System;

namespace SocketIt.Demo
{
    public class CickSocketController : MonoBehaviour
    {

        List<SnapSocket> sockets = new List<SnapSocket>();

        void Update()
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit))
            {
                return;
            }

            SnapSocket socket = hit.collider.GetComponent<SnapSocket>();
            if(socket == null)
            {
                return;
            }

            AddSocket(socket);
        }

        private void AddSocket(SnapSocket socket)
        {
            if (!sockets.Contains(socket))
            {
                sockets.Add(socket);
                ChangeEmissionColor(socket.gameObject, Color.green);
            }

            if(sockets.Count == 2)
            {
                Snap();
                sockets.Clear();
                foreach(SnapSocket snapSocket in sockets)
                {
                    snapSocket.Clear();
                }
            }
        }

        private void Snap()
        {
            sockets[0].SnapModule.IsStatic = false;
            sockets[0].Found(sockets[1]);
            sockets[0].SnapModule.IsStatic = true;

            ChangeEmissionColor(sockets[0].gameObject, Color.black);
            ChangeEmissionColor(sockets[1].gameObject, Color.black);
        }

        private void ChangeEmissionColor(GameObject go, Color color)
        {
            Renderer renderer = go.GetComponent<Renderer>();
            Material mat = renderer.material;
            mat.SetColor("_EmissionColor", color);
        }
    }
}

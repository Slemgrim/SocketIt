using UnityEngine;
using System.Collections;
using SocketIt;
using System;

namespace SocketIt.Demo
{
    public class ConnectHelper : MonoBehaviour
    {
        public Socket socketA;
        public Socket socketB;

        private Module moduleA;
        private Module moduleB;

        void Start()
        {
            moduleA = socketA.Module;
            moduleB = socketB.Module;

            socketA.OnConnect += OnSocketConnect;
            socketA.OnDisconnect += OnSocketDisconnect;

            socketB.OnConnect += OnSocketConnect;
            socketB.OnDisconnect += OnSocketDisconnect;

            moduleA.OnConnect += OnModuleConnect;
            moduleA.OnDisconnect += OnModuleDisconnect;

            moduleB.OnConnect += OnModuleConnect;
            moduleB.OnDisconnect += OnModuleDisconnect;
        }

        void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                if (socketA.ConnectedSocket == socketB)
                {
                    socketA.Disconnect(socketB);
                }
                else
                {
                    socketA.Connect(socketB);
                }
            }
        }

        private void OnModuleDisconnect(Connection con)
        {
            Debug.Log(string.Format(
                "Module {0} Disconnected from {1}",
                con.SocketA.Module.name,
                con.SocketB.Module.name
            ));

            ChangeEmissionColor(con.SocketA.Module.gameObject, Color.black);

        }

        private void OnModuleConnect(Connection con)
        {
            Debug.Log(string.Format(
                "Module {0} connected to {1}",
                con.SocketA.Module.name,
                con.SocketB.Module.name
            ));

            ChangeEmissionColor(con.SocketA.Module.gameObject, Color.green);
        }

        private void OnSocketConnect(Connection con)
        {
            Debug.Log(string.Format(
                "Socket {0}.{1} connected to {2}.{3}",
                con.SocketA.Module.name,
                con.SocketA.name,
                con.SocketB.Module.name,
                con.SocketB.name
            ));

            ChangeEmissionColor(con.SocketA.gameObject, Color.green);
        }

        private void OnSocketDisconnect(Connection con)
        {
            Debug.Log(string.Format(
                "Socket {0}.{1} disconnected from {2}.{3}",
                con.SocketA.Module.name,
                con.SocketA.name,
                con.SocketB.Module.name,
                con.SocketB.name
            ));

            ChangeEmissionColor(con.SocketA.gameObject, Color.black);
        }

        private void ChangeEmissionColor(GameObject go, Color color)
        {
            Renderer renderer = go.GetComponent<Renderer>();
            Material mat = renderer.material;
            mat.SetColor("_EmissionColor", color);
        }
    }
}

using UnityEngine;
using System.Collections;
using SocketIt;
using System;

namespace SocketIt.Example01
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

            moduleA.OnModuleConnected += OnModuleConnect;
            moduleA.OnModuleDisconnected += OnModuleDisconnect;

            moduleB.OnModuleConnected += OnModuleConnect;
            moduleB.OnModuleDisconnected += OnModuleDisconnect;
        }

        void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                if (socketA.GetConnectedSocket() == socketB)
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
                con.Connector.Module.name,
                con.Connectee.Module.name
            ));

            ChangeEmissionColor(con.Connector.Module.gameObject, Color.black);

        }

        private void OnModuleConnect(Connection con)
        {
            Debug.Log(string.Format(
                "Module {0} connected to {1}",
                con.Connector.Module.name,
                con.Connectee.Module.name
            ));

            ChangeEmissionColor(con.Connector.Module.gameObject, Color.green);
        }

		private void OnSocketConnect(Socket socketA, Socket socketBr)
        {
            Debug.Log(string.Format(
                "Socket {0}.{1} connected to {2}.{3}",
                socketA.Module.name,
                socketA.name,
                socketB.Module.name,
                socketB.name
            ));

            ChangeEmissionColor(socketA.gameObject, Color.green);
        }

		private void OnSocketDisconnect(Socket socketA, Socket socketB)
        {
            Debug.Log(string.Format(
                "Socket {0}.{1} disconnected from {2}.{3}",
                socketA.Module.name,
                socketA.name,
                socketB.Module.name,
                socketB.name
            ));

            ChangeEmissionColor(socketA.gameObject, Color.black);
        }

        private void ChangeEmissionColor(GameObject go, Color color)
        {
            Renderer renderer = go.GetComponent<Renderer>();
            Material mat = renderer.material;
            mat.SetColor("_EmissionColor", color);
        }
    }
}

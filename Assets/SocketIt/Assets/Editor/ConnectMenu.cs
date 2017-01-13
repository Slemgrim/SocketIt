using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace SocketIt.Editor
{
    public class SocketItMenu : MonoBehaviour
    {   
        [MenuItem("SocketIt/Connect %&c")]
        static void ConnectSockets(MenuCommand menuCommand)
        {
            Socket activeSocket = GetActiveSocket();
            Socket secondSocket = GetSecondSocket();

            Connect(activeSocket, secondSocket);
            RemoveEmptyComposition();
        }

        private static void Connect(Socket activeSocket, Socket secondSocket)
        {
            RecordModule(activeSocket);
            RecordModule(secondSocket);

            secondSocket.Connect(activeSocket);
        }

        [MenuItem("SocketIt/Disconnect %&x")]
        static void DisconnectSockets(MenuCommand menuCommand)
        {
            Socket activeSocket = GetActiveSocket();
            Socket secondSocket = GetSecondSocket();

            Module activeModule = GetActiveModule();
            Module secondModule = GetSecondModule();

            if(activeSocket != null && secondSocket == null)
            {
                Disconnect(activeSocket, activeSocket.GetConnectedSocket());
            }

            else if (activeSocket != null && secondSocket != null)
            {
                Disconnect(activeSocket, activeSocket.GetConnectedSocket());
            }

            else if(activeModule != null && secondModule == null)
            {
                foreach(Connection connection in new List<Connection>(activeModule.Composition.Connections))
                {
                    connection.Connector.Disconnect(connection.Connectee);
                }
            } else if (activeModule != null && secondModule != null)
            {
                Connection connection = activeModule.GetConnection(secondModule);
                if(connection != null)
                {
                    connection.Connector.Disconnect(connection.Connectee);
                }
            }
            RemoveEmptyComposition();
        }

        private static void Disconnect(Socket activeSocket, Socket secondSocket)
        {
            RecordModule(activeSocket);
            RecordModule(secondSocket);

            if (activeSocket != null)
            {
                secondSocket.Disconnect(activeSocket);
            }
            else
            {
                secondSocket.Disconnect();
            }
        }

        private static Socket GetActiveSocket()
        {
            return Selection.activeGameObject.GetComponent<Socket>();
        }

        private static Socket GetSecondSocket()
        {
            Socket secondSocket = null;
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                if (gameObject == Selection.activeGameObject)
                {
                    continue;
                }

                secondSocket = gameObject.GetComponent<Socket>();
            }

            return secondSocket;
        }

        private static Module GetActiveModule()
        {
            return Selection.activeGameObject.GetComponent<Module>();
        }

        private static Module GetSecondModule()
        {
            Module secondModule = null;
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                if (gameObject == Selection.activeGameObject)
                {
                    continue;
                }

                secondModule = gameObject.GetComponent<Module>();
            }

            return secondModule;
        }

        private static void RecordModule(Socket socket)
        {
            if (socket != null && socket.Module != null)
            {
                Undo.RecordObject(socket.Module, "Connect " + socket.Module.name);
            }
        }

        private static void RemoveEmptyComposition()
        {
            Composition[] compositions = GameObject.FindObjectsOfType<Composition>();
            foreach (Composition composition in compositions)
            {
                if (composition.Modules.Count <= 1)
                {
                    Undo.DestroyObjectImmediate(composition.gameObject);
                }
            }

        }
    }
}

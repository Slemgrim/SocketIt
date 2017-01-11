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
        }

        private static void Connect(Socket activeSocket, Socket secondSocket)
        {
            Composition compositionA = activeSocket.Module.Composition;
            Composition compositionB = secondSocket.Module.Composition;

            RecordModule(activeSocket);
            RecordModule(secondSocket);

            secondSocket.Connect(activeSocket);

            if(activeSocket.Module.Composition != compositionA || activeSocket.Module.Composition != compositionB)
            {
                Undo.RegisterCreatedObjectUndo(activeSocket.Module.Composition.gameObject, "Composition created:" + activeSocket.Module.Composition.name);
            }
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
    }
}

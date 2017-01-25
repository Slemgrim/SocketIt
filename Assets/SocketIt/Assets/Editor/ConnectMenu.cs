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

        [MenuItem("SocketIt/Connect %&c", true)]
        static bool ValidateConnect()
        {
            if (Selection.activeGameObject == null || Selection.gameObjects.Length != 2)
            {
                return false;
            }

            Socket activeSocket = GetActiveSocket();
            Socket secondSocket = GetSecondSocket();

            if(activeSocket == null || secondSocket == null)
            {
                return false;
            }

            if (activeSocket.Module == null || secondSocket.Module == null)
            {
                return false;
            }

            if (activeSocket.IsConnected() || secondSocket.IsConnected())
            {
                return false;
            }

            return true;
        }


        private static void Connect(Socket activeSocket, Socket secondSocket)
        {
            Composition activeOldComposition = activeSocket.Module.Composition;
            Composition secondOldComposition = secondSocket.Module.Composition;

            bool UndoComposition = false;
            if(activeOldComposition == null && secondOldComposition == null)
            {
                UndoComposition = true;
            }

            RecordModule(activeSocket);
            RecordModule(secondSocket);

            activeSocket.Connect(secondSocket);

            if(activeSocket.Module.Composition != activeOldComposition && activeSocket.Module.Composition != secondOldComposition)
            {
                UndoComposition = true;
            }

            if (UndoComposition)
            {
                Undo.RegisterCreatedObjectUndo(secondSocket.Module.Composition.gameObject, "Create new Composition");
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
                    Disconnect(connection.Connector, connection.Connectee);
                }
            } else if (activeModule != null && secondModule != null)
            {
                Connection connection = activeModule.GetConnection(secondModule);
                if(connection != null)
                {
                    Disconnect(connection.Connector, connection.Connectee);
                }
            }
        }

        [MenuItem("SocketIt/Disconnect %&x", true)]
        static bool ValidateDisconnect()
        {
            if (Selection.activeGameObject == null)
            {
                return false;
            }

            Socket activeSocket = GetActiveSocket();

            if (activeSocket == null)
            {
                return false;
            }

            if(activeSocket.IsConnected() == false)
            {
                return false;
            }

            return true;
        }

        private static void Disconnect(Socket activeSocket, Socket secondSocket)
        {
            RecordModule(activeSocket);
            RecordModule(secondSocket);

            if (secondSocket == null)
            {
                return;
            }

            Connection connection = activeSocket.Module.Composition.GetConnection(secondSocket);
            connection.Connector.Disconnect(connection.Connectee);
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

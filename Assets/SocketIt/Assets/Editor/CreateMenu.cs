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
            activeSocket.Connect(secondSocket);
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
                activeSocket.Disconnect();
            }

            else if (activeSocket != null && secondSocket != null)
            {
                activeSocket.Disconnect(secondSocket);
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

        
        /*

        [MenuItem("SocketIt/Snap/Position %&i")]
        static void SnapSocketPosition(MenuCommand menuCommand)
        {
            Socket activeSocket = GetActiveSocket();
            Socket secondSocket = GetSecondSocket();

            Undo.RecordObject(secondSocket.Module.transform, "Snap position to " + activeSocket.name);
            SnapPosition(secondSocket, activeSocket);
        }

        [MenuItem("SocketIt/Snap/Rotation %&o")]
        static void SnapSocketRotation(MenuCommand menuCommand)
        {
            Socket activeSocket = GetActiveSocket();
            Socket secondSocket = GetSecondSocket();

            Undo.RecordObject(secondSocket.Module.transform, "Snap rotation to " + activeSocket.name);
            SnapRotation(secondSocket, activeSocket);
        }


        [MenuItem("SocketIt/Snap/Position Rotation %&u")]
        static void SnapSocketPositionAndRotation(MenuCommand menuCommand)
        {
            Socket activeSocket = GetActiveSocket();
            Socket secondSocket = GetSecondSocket();

            Undo.RecordObject(secondSocket.Module.transform, "Snap position and rotation to " + activeSocket.name);
            SnapRotation(secondSocket, activeSocket);
            SnapPosition(secondSocket, activeSocket);
        }


        [MenuItem("SocketIt/Connect %&c", true)]
        [MenuItem("SocketIt/Snap/Position %&i", true)]
        [MenuItem("SocketIt/Snap/Rotation %&o", true)]
        [MenuItem("SocketIt/Snap/Position Rotation %&u", true)]
        static bool ValidateSocketItAction()
        {
            if (Selection.activeGameObject == null || Selection.gameObjects.Length != 2)
            {
                return false;
            }

            Socket activeSocket = Selection.activeGameObject.GetComponent<Socket>();

            if (activeSocket == null)
            {
                return false;
            }

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                if (gameObject == Selection.activeGameObject)
                {
                    continue;
                }

                Socket socket = gameObject.GetComponent<Socket>();
                if (socket == null || socket.Module == activeSocket.Module)
                {
                    return false;
                }
            }

            return true;
        }

        [MenuItem("SocketIt/Disconnect %&x", true)]
        static bool ValidateDisconnect()
        {
            if (Selection.activeGameObject == null)
            {
                return false;
            }

            Socket activeSocket = Selection.activeGameObject.GetComponent<Socket>();
            Module activeModule = Selection.activeGameObject.GetComponent<Module>();

            if (activeSocket == null && activeModule == null)
            {
                return false;
            }

            return true;
        }

        static void SnapPosition(Socket fromSocket, Socket toSocket)
        {
            Vector3 ownSocketPosition = fromSocket.transform.localPosition;
            ownSocketPosition = fromSocket.Module.transform.rotation * ownSocketPosition;
            fromSocket.Module.transform.position = toSocket.transform.position - ownSocketPosition; 
        }

        static void SnapRotation(Socket fromSocket, Socket toSocket)
        {
            Quaternion forwardRot = Quaternion.FromToRotation(
                fromSocket.transform.forward,
                -toSocket.transform.forward
            );

            fromSocket.Module.transform.rotation = forwardRot * fromSocket.Module.transform.rotation;

            Quaternion upRot = Quaternion.FromToRotation(
                fromSocket.transform.up,
                -toSocket.transform.up
            );

            fromSocket.Module.transform.rotation = upRot * fromSocket.Module.transform.rotation;
        }
        */
  
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
    }
}

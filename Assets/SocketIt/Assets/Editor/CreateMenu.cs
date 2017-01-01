using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace SocketIt.Editor
{
    public class SocketItMenu : MonoBehaviour
    {
        [MenuItem("GameObject/SocketIt/Module", false, 10)]
        static void CreateModule(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Module");
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;

            go.AddComponent<Module>();
        }

        [MenuItem("GameObject/SocketIt/Socket", false, 10)]
        static void CreateSocket(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Socket");
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;

            go.AddComponent<Socket>();
        }

        [MenuItem("SocketIt/Connect %g+c")]
        static void ConnectSockets(MenuCommand menuCommand)
        {
            Socket activeSocket = Selection.activeGameObject.GetComponent<Socket>();
            Socket socket = null;
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                if (gameObject == Selection.activeGameObject)
                {
                    continue;
                }

                socket = gameObject.GetComponent<Socket>();
            }

            if(IsConnected(activeSocket) || IsConnected(socket))
            {
                return;
            }

            if(GetConnection(activeSocket, socket) == null)
            {
                Connection connection = Undo.AddComponent(activeSocket.Module.gameObject, typeof(Connection)) as Connection;
                connection.SocketA = activeSocket;
                connection.SocketB = socket;
                connection.Initiator = socket;
            }

            if (GetConnection(socket, activeSocket) == null)
            {
                Connection connection = Undo.AddComponent(socket.Module.gameObject, typeof(Connection)) as Connection;
                connection.SocketA = socket;
                connection.SocketB = activeSocket;
                connection.Initiator = socket;
            }
        }

        [MenuItem("SocketIt/Disconnect %g+x")]
        static void DisconnectSockets(MenuCommand menuCommand)
        {
            Socket activeSocket = Selection.activeGameObject.GetComponent<Socket>();
            Socket socket = null;
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                if (gameObject == Selection.activeGameObject)
                {
                    continue;
                }

                socket = gameObject.GetComponent<Socket>();
            }

            Connection connection = GetConnection(activeSocket, socket);

            if(connection != null)
            {
                Undo.DestroyObjectImmediate(connection);
            }

            connection = GetConnection(socket, activeSocket);

            if (connection != null)
            {
                Undo.DestroyObjectImmediate(connection);
            }
        }

        [MenuItem("SocketIt/Snap/Position %g+i")]
        static void SnapSocketPosition(MenuCommand menuCommand)
        {
            Socket activeSnapSocket = Selection.activeGameObject.GetComponent<Socket>();

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                if(gameObject == Selection.activeGameObject)
                {
                    continue;
                }

                Socket snapSocket = gameObject.GetComponent<Socket>();

                Undo.RecordObject(snapSocket.Module.transform, "Zero Transform Position");
                SnapPosition(snapSocket, activeSnapSocket);

            }
        }

        [MenuItem("SocketIt/Snap/Rotation %g+o")]
        static void SnapSocketRotation(MenuCommand menuCommand)
        {
            Socket activeSnapSocket = Selection.activeGameObject.GetComponent<Socket>();

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                if (gameObject == Selection.activeGameObject)
                {
                    continue;
                }

                Socket snapSocket = gameObject.GetComponent<Socket>();

                Undo.RecordObject(snapSocket.Module.transform, "Zero Transform Position");
                SnapRotation(snapSocket, activeSnapSocket);
            }
        }


        [MenuItem("SocketIt/Snap/Position Rotation %g+p")]
        static void SnapSocketPositionAndRotation(MenuCommand menuCommand)
        {
            Socket activeSnapSocket = Selection.activeGameObject.GetComponent<Socket>();

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                if (gameObject == Selection.activeGameObject)
                {
                    continue;
                }

                Socket snapSocket = gameObject.GetComponent<Socket>();

                Undo.RecordObject(snapSocket.Module.transform, "Zero Transform Position");
                SnapRotation(snapSocket, activeSnapSocket);
                SnapPosition(snapSocket, activeSnapSocket);
            }
        }


        [MenuItem("SocketIt/Connect %g+c", true)]
        [MenuItem("SocketIt/Disconnect %g+x", true)]
        [MenuItem("SocketIt/Snap/Position %g+i", true)]
        [MenuItem("SocketIt/Snap/Rotation %g+o", true)]
        [MenuItem("SocketIt/Snap/Position Rotation %g+p", true)]

        static bool ValidateSocketItAction()
        {
            if (Selection.activeGameObject == null && Selection.gameObjects.Length != 2)
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

        private static bool IsConnected(Socket socket)
        {
            Connection[] connections = socket.Module.GetComponents<Connection>();

            foreach (Connection connection in connections)
            {
                if (connection.SocketA == socket)
                {
                    return true;
                }
            }

            return false;
        }

        private static Connection GetConnection(Socket socketA, Socket socketB)
        {
            Connection[] connections = socketA.Module.GetComponents<Connection>();

            foreach (Connection connection in connections)
            {
                if (connection.SocketA == socketA && connection.SocketB == socketB)
                {
                    return connection;
                }
            }

            return null;
        }
    }
}

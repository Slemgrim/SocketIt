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
                AddConnection(activeSocket, socket, socket);
            }

            if (GetConnection(socket, activeSocket) == null)
            {
                AddConnection(socket, activeSocket, socket);
            }
            
        }

        private static void AddConnection(Socket socketA, Socket socketB, Socket initiator)
        {
            Connection connection = new Connection();
            connection.SocketA = socketA;
            connection.SocketB = socketB;
            connection.Initiator = initiator;
            Undo.RecordObject(socketA.Module, "Add Connection to " + socketA.Module.name);
            socketA.Module.Connections.Add(connection);
        }

        [MenuItem("SocketIt/Disconnect %&x")]
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
                RemoveConnection(activeSocket, connection);
            }

            connection = GetConnection(socket, activeSocket);

            if (connection != null)
            {
                RemoveConnection(socket, connection);
            }
        }

        private static void RemoveConnection(Socket socket, Connection connection)
        {
            Undo.RecordObject(socket.Module, "Remove Connection from " + socket.Module.name);
            socket.Module.Connections.Remove(connection);
        }

        [MenuItem("SocketIt/Snap/Position %&i")]
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

				Undo.RecordObject(snapSocket.Module.transform, "Snap position to " + activeSnapSocket.name);
                SnapPosition(snapSocket, activeSnapSocket);

            }
        }

        [MenuItem("SocketIt/Snap/Rotation %&o")]
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

				Undo.RecordObject(snapSocket.Module.transform, "Snap rotation to " + activeSnapSocket.name);
                SnapRotation(snapSocket, activeSnapSocket);
            }
        }


        [MenuItem("SocketIt/Snap/Position Rotation %&p")]
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

				Undo.RecordObject(snapSocket.Module.transform, "Snap position and rotation to " + activeSnapSocket.name);
                SnapRotation(snapSocket, activeSnapSocket);
                SnapPosition(snapSocket, activeSnapSocket);
            }
        }


        [MenuItem("SocketIt/Connect %&c", true)]
        [MenuItem("SocketIt/Disconnect %&x", true)]
        [MenuItem("SocketIt/Snap/Position %&i", true)]
        [MenuItem("SocketIt/Snap/Rotation %&o", true)]
        [MenuItem("SocketIt/Snap/Position Rotation %&p", true)]
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
			List<Connection> connections = socket.Module.Connections;

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
			List<Connection> connections = socketA.Module.Connections;

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

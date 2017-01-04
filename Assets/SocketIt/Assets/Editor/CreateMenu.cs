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

            if (IsConnected(activeSocket) || IsConnected(secondSocket))
            {
                return;
            }

            if (GetConnection(activeSocket, secondSocket) == null)
            {
                AddConnection(activeSocket, secondSocket, secondSocket);
            }

            if (GetConnection(secondSocket, activeSocket) == null)
            {
                AddConnection(secondSocket, activeSocket, secondSocket);
            }
        }

        [MenuItem("SocketIt/Disconnect %&x")]
        static void DisconnectSockets(MenuCommand menuCommand)
        {
            Socket activeSocket = GetActiveSocket();
            Socket secondSocket = GetSecondSocket();

            Connection connection = GetConnection(activeSocket, secondSocket);

            if(connection != null)
            {
                RemoveConnection(activeSocket, connection);
            }

            connection = GetConnection(secondSocket, activeSocket);

            if (connection != null)
            {
                RemoveConnection(secondSocket, connection);
            }
        }

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
        [MenuItem("SocketIt/Disconnect %&x", true)]
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

        private static Socket GetActiveSocket()
        {
            return Selection.activeGameObject.GetComponent<Socket>();
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

        private static void RemoveConnection(Socket socket, Connection connection)
        {
            Undo.RecordObject(socket.Module, "Remove Connection from " + socket.Module.name);
            socket.Module.Connections.Remove(connection);
        }
    }
}

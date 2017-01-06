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
            Connection existingConnection = GetConnection(activeSocket);
            if (existingConnection != null)
            {
                Disconnect(existingConnection.SocketA, existingConnection.SocketB);
            }

            existingConnection = GetConnection(secondSocket);
            if (existingConnection != null)
            {
                Disconnect(existingConnection.SocketA, existingConnection.SocketB);
            }

            existingConnection = GetConnection(activeSocket.Module, secondSocket.Module);
            if (existingConnection != null)
            {
                Disconnect(existingConnection.SocketA, existingConnection.SocketB);
            }

            if (GetConnection(activeSocket, secondSocket) == null)
            {
                AddConnection(activeSocket, secondSocket, secondSocket);
            }

            if (GetConnection(secondSocket, activeSocket) == null)
            {
                AddConnection(secondSocket, activeSocket, secondSocket);
            }

            AddNodes(activeSocket, secondSocket);
        }

        private static void AddNodes(Socket activeSocket, Socket secondSocket)
        {
            NodeModule activeNode = activeSocket.Module.GetComponent<NodeModule>();
            NodeModule secondNode = secondSocket.Module.GetComponent<NodeModule>();

            if(activeNode == null || secondNode == null)
            {
                return;
            }

            Undo.RecordObject(activeNode, "Set Nodes on" + activeNode.name);
            Undo.RecordObject(secondNode, "Set Nodes on" + secondNode.name);

            if (activeNode == null || secondNode == null)
            {
                return;
            }

            if (activeNode.ChildNodes.Contains(secondNode))
            {
                return;
            }

            if (secondNode.ParentNode == activeNode) {
                return;
            }

            //We remove old parent Nodes from the second module and replace it with the requested connection
            if (secondNode.ParentNode != null)
            {
                Connection connecton = GetConnection(secondNode.Module, secondNode.ParentNode.Module);
                if (connecton != null)
                {
                    Disconnect(connecton.SocketA, connecton.SocketB);
                    Connect(activeSocket, secondSocket);
                }
            }

            secondNode.ParentNode = activeNode;

            if (!activeNode.ChildNodes.Contains(secondNode))
            {
                activeNode.ChildNodes.Add(secondNode);
            }

            if (DetectCircleReference(activeNode)){
                Disconnect(activeSocket, secondSocket);
                Debug.LogWarningFormat("Can't connect {0} to {1}. Circle referenced NodeModules are not allowed", secondNode.name, activeNode.name);
            }
        }

        private static void RemoveNodes(Socket activeSocket, Socket secondSocket)
        {
            NodeModule activeNode = activeSocket.Module.GetComponent<NodeModule>();
            NodeModule secondNode = secondSocket.Module.GetComponent<NodeModule>();

            if (activeNode == null || secondNode == null)
            {
                return;
            }

            Undo.RecordObject(activeNode, "Remove Nodes on" + activeNode.name);
            Undo.RecordObject(secondNode, "Remove Nodes on" + secondNode.name);

            if (activeNode == null || secondNode == null)
            {
                return;
            }
            
            if(activeNode.ParentNode == secondNode)
            {
                activeNode.ParentNode = null;
            }

            if (secondNode.ParentNode == activeNode)
            {
                secondNode.ParentNode = null;
            }

            if (activeNode.ChildNodes.Contains(secondNode))
            {
                activeNode.ChildNodes.Remove(secondNode);
            }

            if (secondNode.ChildNodes.Contains(activeNode))
            {
                secondNode.ChildNodes.Remove(activeNode);
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
                Disconnect(activeSocket, GetConnectedSocket(activeSocket));
            }

            else if (activeSocket != null && secondSocket != null)
            {
                Disconnect(activeSocket, secondSocket);
            }

            else if(activeModule != null && secondModule == null)
            {
                foreach(Connection connection in new List<Connection>(activeModule.Connections))
                {
                    Disconnect(connection.SocketA, connection.SocketB);
                }
            } else if (activeModule != null && secondModule != null)
            {
                Connection connection = GetConnection(activeModule, secondModule);
                if(connection != null)
                {
                    Disconnect(connection.SocketA, connection.SocketB);
                }
            }
        }

        private static void Disconnect(Socket activeSocket, Socket secondSocket)
        {
            RemoveNodes(activeSocket, secondSocket);
            RemoveConnection(activeSocket, secondSocket);
            RemoveConnection(secondSocket, activeSocket);

        }

        private static Socket GetConnectedSocket(Socket socket)
        {
            List<Connection> connections = socket.Module.Connections;
            foreach(Connection connection in connections)
            {
                if(connection.SocketA == socket)
                {
                    return connection.SocketB;
                }
            }

            return null;
        }

        private static void RemoveConnection(Socket activeSocket, Socket secondSocket)
        {
            Connection connection = GetConnection(activeSocket, secondSocket);

            if (connection != null)
            {
                Undo.RecordObject(activeSocket.Module, "Remove Connection from " + activeSocket.Module.name);
                activeSocket.Module.Connections.Remove(connection);
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

        private static Connection GetConnection(Socket socketA)
        {
            List<Connection> connections = socketA.Module.Connections;

            foreach (Connection connection in connections)
            {
                if (connection.SocketA == socketA)
                {
                    return connection;
                }
            }

            return null;
        }

        private static Connection GetConnection(Module moduleA, Module moduleB)
        {
            List<Connection> connections = moduleA.Connections;
            
            foreach (Connection connection in connections)
            {
                if (connection.SocketB.Module == moduleB)
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

        private static Module GetActiveModule()
        {
            return Selection.activeGameObject.GetComponent<Module>();
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

        private static bool DetectCircleReference(NodeModule module, NodeModule startModule = null)
        {
            if (module.ParentNode == null)
            {
                return false;
            } else if (module.ParentNode == startModule)
            {
                return true;
            } else
            {
                if(startModule == null)
                {
                    startModule = module;
                }
                return DetectCircleReference(module.ParentNode, startModule);
            }
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEditor;

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

        [MenuItem("SocketIt/Connect")]
        static void ConnectSockets(MenuCommand menuCommand)
        {
            //Todo
        }


        [MenuItem("SocketIt/Disconnect")]
        static void DisconnectSockets(MenuCommand menuCommand)
        {
            //Todo
        }

        [MenuItem("SocketIt/Snap")]
        static void SnapSockets(MenuCommand menuCommand)
        {
            Socket activeSnapSocket = Selection.activeGameObject.GetComponent<Socket>();

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                if(gameObject == Selection.activeGameObject)
                {
                    continue;
                }

                Socket snapSocket = gameObject.GetComponent<Socket>();

                Snap(snapSocket, activeSnapSocket);

                Undo.RegisterCompleteObjectUndo(gameObject, "Undo snap");
            }
        }

        [MenuItem("SocketIt/Connect", true)]
        [MenuItem("SocketIt/Disconnect", true)]
        [MenuItem("SocketIt/Snap", true)]
        static bool ValidateSocketItAction()
        {
            if (Selection.activeGameObject == null && Selection.gameObjects.Length < 2)
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

        static void Snap(Socket fromSocket, Socket toSocket)
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

            Vector3 ownSocketPosition = fromSocket.transform.localPosition;
            ownSocketPosition = fromSocket.Module.transform.rotation * ownSocketPosition;
            fromSocket.Module.transform.position = toSocket.transform.position - ownSocketPosition; 
        }
    }
}

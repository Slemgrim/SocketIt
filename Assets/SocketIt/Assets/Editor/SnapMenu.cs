using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace SocketIt.Editor
{
    public class SnapMenu : MonoBehaviour
    {

        [MenuItem("SocketIt/Snap/Position %&u")]
        static void SnapSocketPosition(MenuCommand menuCommand)
        {
            Socket activeSocket = GetActiveSocket();
            Socket secondSocket = GetSecondSocket();

            Snap snap = new Snap(secondSocket, activeSocket);
            Snap(snap, true, false, false);
        }

        [MenuItem("SocketIt/Snap/Forward Rotation %&i")]
        static void SnapSocketRotation(MenuCommand menuCommand)
        {
            Socket activeSocket = GetActiveSocket();
            Socket secondSocket = GetSecondSocket();

            Snap snap = new Snap(secondSocket, activeSocket);
            Snap(snap, false, true, false);
        }


        [MenuItem("SocketIt/Snap/Up Rotation %&o")]
        static void SnapSocketPositionAndRotation(MenuCommand menuCommand)
        {
            Socket activeSocket = GetActiveSocket();
            Socket secondSocket = GetSecondSocket();

            Snap snap = new Snap(secondSocket, activeSocket);
            Snap(snap, false, false, true);
        }

        [MenuItem("SocketIt/Snap/Position %&u", true)]
        [MenuItem("SocketIt/Snap/Forward Rotation %&i", true)]
        [MenuItem("SocketIt/Snap/Up Rotation %&o", true)]
        static bool ValidateSnappingAction()
        {
            if (Selection.activeGameObject == null || Selection.gameObjects.Length != 2)
            {
                return false;
            }

            Socket activeSocket = GetActiveSocket();
            Socket secondSocket = GetSecondSocket();

            if (activeSocket == null || secondSocket == null)
            {
                return false;
            }

            return true;
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

        private static void Snap(Snap snap, bool snapPositon, bool snapForward, bool snapUp)
        {
            if (snap.SocketA != null && snap.SocketA.Module != null)
            {
                Undo.RecordObject(snap.SocketA.Module.transform, "Snap Sockets");
            }
            SnapTransform target = snap.GetTargetTransform(snapPositon, snapForward, snapUp);
            snap.SocketA.Module.transform.position = target.position;
            snap.SocketA.Module.transform.rotation = target.rotation;
        }
    }
}

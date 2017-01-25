using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace SocketIt.Editor
{
    [CustomEditor(typeof(Socket))]
    public class SocketInspector : UnityEditor.Editor
    {
        Socket socketA;
        static Socket snapSocketB;
        static Socket connectSocketB;

        static bool SnapPosition = true;
        static bool SnapUp = true;
        static bool SnapForward = true;

        public static bool ShowRotationHandle = false;

        public override void OnInspectorGUI()
        {
            HandleUtility.Repaint();
            DrawDefaultInspector();

            socketA = (Socket)target;

            if(socketA.Module == null)
            {
                return;
            }

            EditorGUILayout.Space();
            bool changedRotationHandle = EditorGUILayout.Toggle("Show Rotation Handle", ShowRotationHandle);
            if (changedRotationHandle != ShowRotationHandle)
            {
                SceneView.RepaintAll();
                ShowRotationHandle = changedRotationHandle;
            }

            if (socketA.IsConnected())
            {
                Disconnect();
            } else
            {
                Connect();
            }

            SnapField();
        }

        void OnSceneGUI()
        {
            socketA = (Socket)target;
            if (ShowRotationHandle == false || socketA.Module == null)
            {
                return;
            }

            Quaternion newRoation = Handles.RotationHandle(socketA.transform.rotation, socketA.transform.position);
            if(newRoation != socketA.transform.rotation)
            {
                RecordModules();

                //Save old parents
                Transform oldSocketParent = socketA.transform.parent;
                Transform oldModuleParent = socketA.Module.transform.parent;

                //Switch parents old parents
                socketA.Module.transform.SetParent(null);
                socketA.transform.SetParent(null);
                socketA.Module.transform.SetParent(socketA.transform);

                //Apply new rotation to socket. Module follows since it has the socket as parent now
                socketA.transform.rotation = newRoation;

                //Restore old parents
                socketA.Module.transform.SetParent(null);
                socketA.transform.SetParent(null);
                socketA.transform.SetParent(oldSocketParent);
                socketA.Module.transform.SetParent(oldModuleParent);
            }
        }

        private void SnapField()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Snap To Socket");

            snapSocketB = (Socket)EditorGUILayout.ObjectField(snapSocketB, typeof(Socket), true);

            EditorGUI.BeginDisabledGroup(snapSocketB == null);
            if (GUILayout.Button("Snap"))
            {
                Snap snap = new Snap(socketA, snapSocketB);
                Snap(snap);
            }
            EditorGUI.EndDisabledGroup();

            SnapPosition = EditorGUILayout.Toggle("Position", SnapPosition);
            SnapUp = EditorGUILayout.Toggle("Up", SnapUp);
            SnapForward = EditorGUILayout.Toggle("Forward", SnapForward);

            EditorGUILayout.EndVertical();
        }


        public void Connect()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Connect to Socket");
            EditorGUILayout.BeginHorizontal();

            connectSocketB = (Socket)EditorGUILayout.ObjectField(connectSocketB, typeof(Socket), true);

            EditorGUILayout.EndHorizontal();
            EditorGUI.BeginDisabledGroup(socketA == false || connectSocketB == false || !socketA.Module.Sockets.Contains(socketA) || socketA.Module.Sockets.Contains(connectSocketB));
            if (GUILayout.Button("Connect"))
            {
                RecordModules();
                connectSocketB.Connect(socketA);
        
                socketA = null;
                connectSocketB = null;
            }

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
        }

        public void Disconnect()
        {
       
            if (GUILayout.Button("Disconnect"))
            {
                RecordModules();
                socketA.Disconnect();
            }
        }

        private void Snap(Snap snap)
        {
            RecordModules();
            SnapTransform target = snap.GetTargetTransform(SnapPosition, SnapForward, SnapUp);
            snap.SocketA.Module.transform.position = target.position;
            snap.SocketA.Module.transform.rotation = target.rotation;
        }

        private void RecordModules()
        {
            if (socketA != null && socketA.Module != null)
            {
                Undo.RecordObject(socketA.Module, "Connect");
            }

            if (snapSocketB != null && snapSocketB.Module != null)
            {
                Undo.RecordObject(snapSocketB.Module, "Snap");
            }

            if (connectSocketB != null && connectSocketB.Module != null)
            {
                Undo.RecordObject(connectSocketB.Module, "Connect");
            }
        }
    }
}

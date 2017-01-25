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
        Socket socketB;

        bool SnapPosition = true;
        bool SnapUp = true;
        bool SnapForward = true;

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
                Undo.RecordObject(socketA.transform, "Rotate by Socket");
                Undo.RecordObject(socketA.Module.transform, "Rotate by Socket");

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

            socketB = (Socket)EditorGUILayout.ObjectField(socketB, typeof(Socket), true);

            if (GUILayout.Button("Snap"))
            {
                Debug.Log("Snap");
                socketB.Snap(socketA);
            }

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

            socketB = (Socket)EditorGUILayout.ObjectField(socketB, typeof(Socket), true);

            EditorGUILayout.EndHorizontal();
            EditorGUI.BeginDisabledGroup(socketA == false || socketB == false || !socketA.Module.Sockets.Contains(socketA) || socketA.Module.Sockets.Contains(socketB));
            if (GUILayout.Button("Connect"))
            {
                RecordModules();
                socketB.Connect(socketA);
        
                socketA = null;
                socketB = null;
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

        private void RecordModules()
        {
            if (socketA != null && socketA.Module != null)
            {
                Undo.RecordObject(socketA.Module, "Connect");
            }

            if (socketB != null && socketB.Module != null)
            {
                Undo.RecordObject(socketB.Module, "Connect");
            }
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace SocketIt.Editor
{
    [CustomEditor(typeof(Socket))]
    public class SocketInspector : UnityEditor.Editor
    {
        Socket socketB;
        Socket socketA;

        bool SnapPosition = true;
        bool SnapUp = true;
        bool SnapForward = true;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            socketA = (Socket)target;

            if(socketA.Module == null)
            {
                return;
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

        private void SnapField()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Snap To Socket");

            socketB = (Socket)EditorGUILayout.ObjectField(socketB, typeof(Socket), true);

            if (GUILayout.Button("Snap"))
            {

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

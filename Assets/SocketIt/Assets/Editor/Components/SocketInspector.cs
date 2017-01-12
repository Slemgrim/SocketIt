using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SocketIt.Editor
{
    [CustomEditor(typeof(Socket))]
    public class SocketInspector : UnityEditor.Editor
    {
        Socket socketB;
        Socket socketA;

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
                Connect("Connect other Socket", "Connect");
            }
        }

        public void Connect(string label, string buttonLabel)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(label);
            EditorGUILayout.BeginHorizontal();

            socketB = (Socket)EditorGUILayout.ObjectField(socketB, typeof(Socket), true);

            EditorGUILayout.EndHorizontal();
            EditorGUI.BeginDisabledGroup(socketA == false || socketB == false || !socketA.Module.Sockets.Contains(socketA) || socketA.Module.Sockets.Contains(socketB));
            if (GUILayout.Button(buttonLabel))
            {
                RecordModules();
                socketA.Connect(socketB);
        
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

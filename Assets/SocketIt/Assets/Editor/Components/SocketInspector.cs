using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SocketIt.Editor
{
    [CustomEditor(typeof(Socket))]
    public class SocketInspector : UnityEditor.Editor
    {
        Socket otherSocket;
        Socket socket;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            socket = (Socket)target;

            Connect();
        }

        private void Connect()
        {
            EditorGUILayout.BeginHorizontal("Button");

            EditorGUI.BeginDisabledGroup(otherSocket == false || otherSocket == socket);
            if (GUILayout.Button("Connect"))
            {
                socket.Connect(otherSocket);
            }
            EditorGUI.EndDisabledGroup();

            otherSocket = (Socket)EditorGUILayout.ObjectField(otherSocket, typeof(Socket), true);

            EditorGUILayout.EndHorizontal();
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using SocketIt;

[CanEditMultipleObjects]
[CustomEditor(typeof(Module))]
public class ModuleInspector : UnityEditor.Editor
{
    Module module;

    Socket socketA;
    Socket socketB;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        module = (Module)target;
        
        Undo.RecordObject(module, "Edit module " + module);

        if (module.Composition == null && module.Sockets.Count != 0)
        {
            Connect("Create Composition", "Create");
        }
        else if(module.Composition != null && module.Sockets.Count != 0)
        {
            Connect("Connect Modules", "Connect");
        }
    }

    public void Connect(string label, string buttonLabel)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField(label);
        EditorGUILayout.BeginHorizontal();

        socketA = (Socket)EditorGUILayout.ObjectField(socketA, typeof(Socket), true);
        socketB = (Socket)EditorGUILayout.ObjectField(socketB, typeof(Socket), true);

        EditorGUILayout.EndHorizontal();
        EditorGUI.BeginDisabledGroup(socketA == false || socketB == false || !socketA.Module.Sockets.Contains(socketA) || socketA.Module.Sockets.Contains(socketB));
        if (GUILayout.Button(buttonLabel))
        {
            Undo.RecordObject(socketA.Module, "Connect");
            Undo.RecordObject(socketB.Module, "Connect");

            socketA.Connect(socketB);

            if (socketA.Module.Composition != null)
            {
                Undo.RegisterCreatedObjectUndo(socketA.Module.Composition.gameObject, "Create composition");
            }
        }

        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndVertical();
    }
}


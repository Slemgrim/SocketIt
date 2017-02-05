using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SocketIt.Editor
{
    [CustomEditor(typeof(Composition))]
    public class CompositionInspector : UnityEditor.Editor
    {
        Composition composition;


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            composition = (Composition)target;
            if (composition.Modules == null)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            composition.Origin = (Module) EditorGUILayout.ObjectField("Origin", composition.Origin, typeof(Module), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Modules: " + composition.Modules.Count);
            EditorGUILayout.LabelField("Connections: " + composition.Connections.Count);

            if(composition.Modules.Count > 0)
            {
                if (GUILayout.Button("Remove all modules"))
                {
                    composition.Clear();
                }
            }
        }
    }
}

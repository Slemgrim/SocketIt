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

            if(composition.Modules != null)
            {
                if (GUILayout.Button("Free Modules"))
                {
                    composition.FreeModules();
                }
            }
        }
    }
}

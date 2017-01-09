using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SocketIt.Editor
{
    [CustomEditor(typeof(Module))]
    public class ModuleInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();


            Module module = (Module)target;
            Undo.RecordObject(module, "Edit module " + module);

            if (module.Composition == null)
            {
                if (GUILayout.Button("Create composition"))
                {
                    Composition.CreateComposition((Module)target);
                }
              
            } else if(module.Composition.Origin == module)
            {

                if (GUILayout.Button("Destroy composition"))
                {
                    Composition composition = module.Composition;
                    module.Composition.FreeModules();
                    DestroyImmediate(composition.gameObject);
                }
            }
        }
    }
}

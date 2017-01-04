using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace SocketIt.Editor
{
    public class Components : MonoBehaviour
    {
        [MenuItem("GameObject/SocketIt/Module", false, 10)]
        static void CreateModule(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Module");
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;

            go.AddComponent<Module>();
        }

        [MenuItem("GameObject/SocketIt/Socket", false, 10)]
        static void CreateSocket(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Socket");
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;

            go.AddComponent<Socket>();
        }
    }
}

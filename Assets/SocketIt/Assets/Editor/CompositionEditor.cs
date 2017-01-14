using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace SocketIt.Editor {
	public class CompositionEditor : EditorWindow {


        static Module activeModule;
        static Socket activeSocket;
        static Composition activeComposition;
        Vector2 scrollPos;

        [MenuItem ("Window/Composition")]
		public static void ShowWindow () {
			EditorWindow.GetWindow(typeof(CompositionEditor), false, "Composition");
		}

		void OnGUI () {
			if (activeComposition != null) {
				RenderCompositionEditor ();
			} else {
				RenderEmpty ();
			}	
		}

		void RenderCompositionEditor(){
            EditorGUILayout.BeginScrollView(scrollPos);

            EditorGUILayout.LabelField("Connections: " + activeComposition.Connections.Count);
            EditorGUILayout.LabelField("Modules: " + activeComposition.Modules.Count);

            if (GUILayout.Button("Origin: " + activeComposition.Origin.name, "Label", GUILayout.ExpandWidth(false)))
            {
                Selection.activeGameObject = activeComposition.Origin.gameObject;
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            Connection toRemove = null;

            List<Connection> selectedConnections = new List<Connection>();
            if(activeSocket != null)
            {
                selectedConnections.Add(activeComposition.GetConnection(activeSocket));
            } else if(activeModule != null)
            {
                selectedConnections.AddRange(activeComposition.GetConnections(activeModule));
            }

            foreach (Connection connection in activeComposition.Connections)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginHorizontal("Box", GUILayout.ExpandWidth(true));

                GUIStyle style = new GUIStyle(GUI.skin.label);
                if (selectedConnections.Contains(connection))
                {
                    style.normal.textColor = Color.blue;
                    style.fontStyle = FontStyle.Bold;
                }

                if (GUILayout.Button(connection.Connector.Module.name + " -> " + connection.Connector.name, style, GUILayout.ExpandWidth(false)))
                {
                    Selection.activeGameObject = connection.Connector.gameObject;
                }
                
                EditorGUILayout.EndHorizontal();

                    if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                {
                    toRemove = connection;
                }

                EditorGUILayout.BeginHorizontal("Box", GUILayout.ExpandWidth(true));

                if (GUILayout.Button(connection.Connectee.Module.name + " -> " + connection.Connectee.name, style, GUILayout.ExpandWidth(true)))
                {
                    Selection.activeGameObject = connection.Connectee.gameObject;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndHorizontal();

            }

            if(toRemove != null)
            {
                toRemove.Connector.Disconnect(toRemove.Connectee);
            }

            if(activeComposition.Connections.Count == 0)
            {
                Undo.DestroyObjectImmediate(activeComposition.gameObject);
            }

            EditorGUILayout.EndScrollView();
        }

	 	void RenderEmpty(){
			EditorGUILayout.LabelField ("Select a Composition or a Module/Socket connected to a Composition");
		}

		void OnSelectionChange() {
			if (Selection.activeGameObject == null) {
				activeComposition = null;
				return;
			}	

			Composition composition = Selection.activeGameObject.GetComponent<Composition> ();

			if (composition == null) {
				composition = GetCompositionFromModule ();
			}

			if (composition == null) {
				composition = GetCompositionFromSocket ();
			}
				
			activeComposition = composition;
		}

		static Composition GetCompositionFromModule ()
		{	
			Module module = Selection.activeGameObject.GetComponent<Module> ();
			if (module == null) {
				return null;
			}
            activeModule = module;

            return module.Composition;
		}

		static Composition GetCompositionFromSocket ()
		{	
			Socket socket = Selection.activeGameObject.GetComponent<Socket> ();

			if (socket == null) {
				return null;
			}

            activeSocket = socket;

			Module module = socket.Module;

			if (module == null) {
				return null;
			}	

			return module.Composition;
		}

		public void OnInspectorUpdate()
		{
			Repaint();
		}
	}
}	

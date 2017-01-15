using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace SocketIt.Editor {
	public class CompositionEditor : EditorWindow {


        static Module activeModule;
        static Socket activeSocket;
        static Composition activeComposition;
        static Vector2 scrollPos = new Vector2();
        static bool showOnlySelected = false;

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

		void RenderCompositionEditor()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            EditorGUILayout.LabelField("Connections: " + activeComposition.Connections.Count);
            EditorGUILayout.LabelField("Modules: " + activeComposition.Modules.Count);

            if (activeComposition.Origin != null)
            {
                if (GUILayout.Button("Origin: " + activeComposition.Origin.name, "Label", GUILayout.ExpandWidth(false)))
                {
                    Selection.activeGameObject = activeComposition.Origin.gameObject;
                }
            }

            showOnlySelected = EditorGUILayout.Toggle("Show only selected", showOnlySelected);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            Connection toRemove = null;

            List<Connection> selectedConnections = GetSelectedConnections();
            List<Connection> connections = activeComposition.Connections;

            if (showOnlySelected)
            {
                connections = selectedConnections;
            }

            foreach (Connection connection in connections)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label);
                if (selectedConnections.Contains(connection))
                {
                    style.fontStyle = FontStyle.Bold;
                }

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginHorizontal("Box", GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(connection.Connector.Module.name + " -> " + connection.Connector.name, style, GUILayout.ExpandWidth(false));

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Select", GUILayout.ExpandWidth(false)))
                {
                    Selection.activeGameObject = connection.Connector.gameObject;
                }

                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                {
                    toRemove = connection;
                }

                EditorGUILayout.BeginHorizontal("Box", GUILayout.ExpandWidth(true));


                if (GUILayout.Button("Select", GUILayout.ExpandWidth(false)))
                {
                    Selection.activeGameObject = connection.Connectee.gameObject;
                }

                GUILayout.FlexibleSpace();

                EditorGUILayout.LabelField(connection.Connectee.Module.name + " -> " + connection.Connector.name, style, GUILayout.ExpandWidth(false));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndHorizontal();

            }

            if (toRemove != null)
            {
                toRemove.Connector.Disconnect(toRemove.Connectee);
            }

            EditorGUILayout.EndScrollView();
        }

        private static List<Connection> GetSelectedConnections()
        {
            List<Connection> selectedConnections = new List<Connection>();
            if (activeSocket != null)
            {
                selectedConnections.Add(activeComposition.GetConnection(activeSocket));
            }
            else if (activeModule != null)
            {
                selectedConnections.AddRange(activeComposition.GetConnections(activeModule));
            }

            return selectedConnections;
        }

        void RenderEmpty(){
			EditorGUILayout.LabelField ("Select a Composition or a Module/Socket connected to a Composition");
		}

		void OnSelectionChange() {
            activeComposition = null;
            activeModule = null;
            activeSocket = null;

            if (Selection.activeGameObject == null) {
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

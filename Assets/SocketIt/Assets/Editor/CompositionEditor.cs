using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SocketIt.Editor {
	public class CompositionEditor : EditorWindow {

		Composition activeComposition;

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
			EditorGUILayout.LabelField ("Active Composition: " + activeComposition.name);
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
			return module.Composition;
		}

		static Composition GetCompositionFromSocket ()
		{	
			Socket socket = Selection.activeGameObject.GetComponent<Socket> ();

			if (socket == null) {
				return null;
			}	

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

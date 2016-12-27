using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SocketIt.Examples
{
    public class LoadSceneOnClick : MonoBehaviour
    {

        public string scene;
		public Color activeColor;
		public Color inactiveColor;
		public Gui gui;

		public string title;
		public string description;

		private Text text;

		void Awake(){
			text = GetComponentInChildren<Text> ();

			UpdateMenu ();
		}	

		public void UpdateMenu(){
			if ((SceneManager.GetActiveScene ().name == SceneManager.GetSceneByName (scene).name)) {
				SetActive ();
			} else {
				SetInactive ();
			}
		}

        public void OnClick()
        {
            SceneManager.LoadScene(scene);
			UpdateMenu ();
        }

		private void SetActive(){
			text.color = activeColor;
			gui.SetDescription (title, description);
		}

		private void SetInactive(){
			text.color = inactiveColor;
		}

    }
}

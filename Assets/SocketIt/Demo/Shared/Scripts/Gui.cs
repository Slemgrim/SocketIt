using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

namespace SocketIt.Examples {
	public class Gui : MonoBehaviour {

		public string startScene = "Intro";
		private List<LoadSceneOnClick> sceneItems;

		public Text Title;
		public Text Description;

		public void Awake(){
			DontDestroyOnLoad(transform.gameObject);
			sceneItems = new List<LoadSceneOnClick> (GetComponentsInChildren<LoadSceneOnClick>());
		}

		public void Start(){
			SceneManager.LoadScene(startScene);
			SceneManager.sceneLoaded += OnSceneLoad;
		}

		private void OnSceneLoad(Scene scene, LoadSceneMode mode){
			foreach (LoadSceneOnClick sceneItem in sceneItems) {
				sceneItem.UpdateMenu ();
			}
		}

		public void SetDescription(string title, string description){
			Title.text = title;
			Description.text = description;
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

namespace SocketIt.Examples {
	public class Gui : MonoBehaviour {

        public static Gui instance;

		private List<LoadSceneOnClick> sceneItems;

		public Text Title;
		public Text Description;
        public Text Tutorial;

        public void Awake(){
            if(instance == null)
            {
                DontDestroyOnLoad(transform.gameObject);
                sceneItems = new List<LoadSceneOnClick>(GetComponentsInChildren<LoadSceneOnClick>());
                instance = this;
            } else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

		public void Start(){
			SceneManager.sceneLoaded += OnSceneLoad;
		}

		private void OnSceneLoad(Scene scene, LoadSceneMode mode){
			foreach (LoadSceneOnClick sceneItem in sceneItems) {
				sceneItem.UpdateMenu ();
			}
		}

		public void SetDescription(string title, string description, string tutorial){
			Title.text = title;
			Description.text = description;
            Tutorial.text = tutorial;
		}
	}
}

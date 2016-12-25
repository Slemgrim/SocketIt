using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace SocketIt.Examples
{
    public class LoadSceneOnClick : MonoBehaviour
    {

        public string Scene;

        public void OnClick()
        {
            SceneManager.LoadScene(Scene);
        }

    }
}

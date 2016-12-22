using UnityEngine;
using System.Collections;

public class LoadSceneOnClick : MonoBehaviour {

    public string Scene;
    
    public void OnClick()
    {
        Application.LoadLevel(Scene);
    }

}

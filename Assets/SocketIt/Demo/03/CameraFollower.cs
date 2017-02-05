using UnityEngine;

public class CameraFollower : MonoBehaviour {

    public Camera cam;
    public GameObject target;

    void Update () {
        cam.transform.LookAt(target.transform.position);
	}
}

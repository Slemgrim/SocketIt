using UnityEngine;
using System.Collections;

public class ParentRotation : MonoBehaviour {

    public GameObject child;
    public GameObject target;

    void Start () {
        GameObject parent = child.transform.parent.gameObject;

        // Get the difference between child and target
        Quaternion diff = Quaternion.Inverse(child.transform.rotation) * target.transform.rotation;

        // Get the difference between parent and its child
        Quaternion offset = Quaternion.Inverse(child.transform.rotation) * parent.transform.rotation;

        parent.transform.rotation *= diff * Quaternion.Inverse(offset);
	}
}

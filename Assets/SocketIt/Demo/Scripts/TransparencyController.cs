using UnityEngine;
using System.Collections;

public class TransparencyController : MonoBehaviour {

    public Material TransparentMaterial;
    public Material DefaultMaterial;

    private Renderer renderer;

    public void Awake()
    {
        renderer = GetComponent<Renderer>();
        DefaultMaterial = renderer.material;
    }

    public void MakeTransparent()
    {
       renderer.material = TransparentMaterial;
    }

    public void RestoreDefault()
    {
        renderer.material = DefaultMaterial;
    }
}

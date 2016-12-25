using UnityEngine;
using System.Collections;

namespace SocketIt.Examples
{
    public class TransparencyController : MonoBehaviour
    {

        public Material TransparentMaterial;
        public Material DefaultMaterial;

        private Renderer rend;

        public void Awake()
        {
            rend = GetComponent<Renderer>();
            DefaultMaterial = rend.material;
        }

        public void MakeTransparent()
        {
            rend.material = TransparentMaterial;
        }

        public void RestoreDefault()
        {
            rend.material = DefaultMaterial;
        }
    }
}

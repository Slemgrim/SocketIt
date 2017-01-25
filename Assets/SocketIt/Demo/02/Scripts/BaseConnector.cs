using UnityEngine;
using System.Collections;
using System;

namespace SocketIt.Example07
{
    public class BaseConnector : MonoBehaviour
    {
        private Renderer rend;

        public void Awake()
        {
            rend = GetComponent<Renderer>();
        }

        private void ChangeEmissionColor(Color color)
        {
            Material mat = rend.material;
            mat.SetColor("_EmissionColor", color);
        }
    }

}


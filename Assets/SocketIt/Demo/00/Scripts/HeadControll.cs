using UnityEngine;
using System.Collections;
using System;

namespace SocketIt.Example00
{ 
    public class HeadControll : MonoBehaviour {

        private BaseModule baseModule;

        public Material activeMaterial;
        public Material inactiveMaterial;

	    void Awake() {
            baseModule = GetComponent<BaseModule>();
            baseModule.OnNodeConnected += OnNodeConnected;
            baseModule.OnNodeConnectedIndirect += OnNodeConnected;

            baseModule.OnNodeDisconnected += OnNodeDisconnected;
            baseModule.OnNodeDisconnectedIndirect += OnNodeDisconnected;
        }

        private void OnNodeConnected(ModuleNode node)
        {
            node.GetComponent<Renderer>().material = activeMaterial;
        }

        private void OnNodeDisconnected(ModuleNode node)
        {
            node.GetComponent<Renderer>().material = inactiveMaterial;
        }
    }
}

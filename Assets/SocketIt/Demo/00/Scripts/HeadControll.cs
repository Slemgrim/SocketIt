using UnityEngine;
using System.Collections;
using System;

namespace SocketIt.Example00
{ 
    public class HeadControll : MonoBehaviour {

        public Module Module;

        public Material activeMaterial;
        public Material inactiveMaterial;

        public Composition Composition;

	    void Awake() {
            Module = GetComponent<Module>();
            Module.OnCompositionChanged += OnCompositonChanged;
        }

        private void OnCompositonChanged(Composition composition)
        {
            if(Composition != null)
            {
                Composition.OnModuleAdded -= OnModuleConnect;
                Composition.OnModuleRemoved -= OnModuleDisconnect;
            }

            if (composition != null)
            {
                composition.OnModuleAdded += OnModuleConnect;
                composition.OnModuleRemoved += OnModuleDisconnect;
            } 

            Composition = composition;
        }

        private void ActivateAll()
        {
           if(Composition == null)
            {
                return;
            }

            foreach (Module module in Composition.Modules)
            {
                module.GetComponent<Renderer>().material = activeMaterial;
            }
        }

        private void OnModuleDisconnect(Module module)
        {
            module.GetComponent<Renderer>().material = inactiveMaterial;
        }

        private void OnModuleConnect(Module module)
        {
            module.GetComponent<Renderer>().material = activeMaterial;
        }
    }
}

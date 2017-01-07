using UnityEngine;
using System.Collections;
using System;

namespace SocketIt.Example00
{ 
    public class HeadControll : MonoBehaviour {

        public Module Module;

        public Material activeMaterial;
        public Material inactiveMaterial;

        public Composition composition;

	    void Awake() {
            Module = GetComponent<Module>();
            Module.OnCompositionSet += OnCompositonSet;
            Module.OnCompositionRemoved += OnCompositonRemoved;
        }

        private void OnCompositonRemoved(Composition composition)
        {
            //Debug.Log("Remove Composition");

            composition.OnModuleConnect -= OnModuleConnect;
            composition.OnModuleDisconnect -= OnModuleDisconnect;
            this.composition = null;
        }

        private void OnCompositonSet(Composition composition)
        {
            //Debug.Log("Set Composition");
            this.composition = composition;
            composition.OnModuleConnect += OnModuleConnect;
            composition.OnModuleDisconnect += OnModuleDisconnect;

            ActivateAll();
        }

        private void ActivateAll()
        {
           if(composition == null)
            {
                return;
            }

            //Debug.Log("activate all");

            foreach (Module module in composition.Modules)
            {
                module.GetComponent<Renderer>().material = activeMaterial;
            }
        }

        private void OnModuleDisconnect(Module module)
        {
            //Debug.Log("deactivate " + module.name);
            module.GetComponent<Renderer>().material = inactiveMaterial;
        }

        private void OnModuleConnect(Module module)
        {
            //Debug.Log("activate " + module.name);
            module.GetComponent<Renderer>().material = activeMaterial;
        }
    }
}

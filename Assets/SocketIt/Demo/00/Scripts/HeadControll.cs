using UnityEngine;
using System.Collections;
using System;

namespace SocketIt.Example00
{ 
    public class HeadControll : MonoBehaviour {
        public Material activeMaterial;
        public Material inactiveMaterial;

        public Composition Composition;

        public void OnCompositonChanged(Composition composition)
        {
            if(Composition != null)
            {
                Composition.OnModuleAdded.RemoveListener(OnModuleConnect);
                Composition.OnModuleRemoved.RemoveListener(OnModuleDisconnect);
            }

            if (composition != null)
            {
                composition.OnModuleAdded.AddListener(OnModuleConnect);
                composition.OnModuleRemoved.AddListener(OnModuleDisconnect);
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SocketIt
{
    [DisallowMultipleComponent]
    public class Composition : MonoBehaviour
    {
        public List<Module> Modules = new List<Module>();
        public Module Center = null;

        public delegate void CompositionEvent(Module module);
        public event CompositionEvent OnModuleConnect;
        public event CompositionEvent OnModuleDisconnect;

        private static int MaxCompositionId = 0;

        public void AddModule(Module module)
        {
            //Debug.Log(name + " Add " + module.name);

            if (!Modules.Contains(module))
            {
                Modules.Add(module);
                //Debug.Log(name + " Add " + module.name);
                if (OnModuleConnect != null)
                {
                    OnModuleConnect(module);
                }
            }
        }

        public void RemoveModule(Module module)
        {
            if(module == Center)
            {
                return;
            }

            if (Modules.Contains(module))
            {
                Modules.Remove(module);
                //Debug.Log(name + " Remove " + module.name);

                if (OnModuleDisconnect != null)
                {
                    OnModuleDisconnect(module);
                }
            }
            
            if (Modules.Count < 2)
            {
                Center.Composition = null;
                Destroy(gameObject);
            }
        }

        void OnDrawGizmos()
        {
            SocketItGizmo.DrawComposition(this);
        }

        public static Composition CreateNew(Module center)
        {
            int newId = ++MaxCompositionId;
            GameObject gameObject = new GameObject("Composition " + newId);
            Composition composition = gameObject.AddComponent<Composition>();
            composition.Center = center;

            return composition;
        }

        public List<Module> GetConnected(Module startModule, List<Module> reached = null)
        {
            if (reached == null)
            {
                reached = new List<Module>();
            }

            if (!reached.Contains(startModule)){
                reached.Add(startModule);
            }

            List<Module> toCheck = new List<Module>();

            foreach (Connection connection in startModule.Connections)
            {
                toCheck.Add(connection.SocketB.Module);
            }

            foreach (Module module in toCheck)
            {
                if (reached.Contains(module))
                {
                    continue;
                }

                GetConnected(module, reached);
               
            }

            return reached;
        }

        public bool IsConnected(Module module, List<Module> modules = null)
        {
            if (module == Center)
            {
                return true;
            }

            if (modules == null)
            {
                modules = new List<Module>();
            }
            modules.AddRange(GetConnected(module));

            foreach (Module toCheck in modules)
            {
                if(toCheck != module && toCheck == Center)
                {
                    return true;
                }
            } 

            return false;
        }

        internal void Delete()
        {
            foreach(Module module in Modules)
            {
                module.Composition = null;
            }

            Destroy(gameObject);
        }
    }
}

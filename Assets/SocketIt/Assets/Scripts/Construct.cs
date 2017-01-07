using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SocketIt
{
    public class Construct : MonoBehaviour
    {
        public List<Module> Modules = new List<Module>();
        public Module Center = null;

        private static int MaxConstructId = 0;

        public void AddModule(Module module)
        {
            //Debug.Log(name + " Add " + module.name);

            if (!Modules.Contains(module))
            {
                Modules.Add(module);
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
            }
            
            if (Modules.Count < 2)
            {
                Center.Construct = null;
                Destroy(gameObject);
            }
        }

        void OnDrawGizmos()
        {
            SocketItGizmo.DrawConstruct(this);
        }

        public static Construct CreateNew(Module center)
        {
            int newId = ++MaxConstructId;
            GameObject gameObject = new GameObject("Construct " + newId);
            Construct construct = gameObject.AddComponent<Construct>();
            construct.Center = center;

            List<Module> modules = new List<Module>();
            construct.IsConnected(center);

            foreach (Module module in modules)
            {
                construct.AddModule(module);
            } 

            return construct;
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
                module.Construct = null;
            }

            Destroy(gameObject);
        }
    }
}

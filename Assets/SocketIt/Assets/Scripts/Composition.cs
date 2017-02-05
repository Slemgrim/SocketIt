using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace SocketIt
{
    [DisallowMultipleComponent]
    public class Composition : MonoBehaviour
    {
        public Module Origin = null;

        [System.Serializable]
        public class ModuleEvent : UnityEvent<Module> { }
        public ModuleEvent OnModuleAdded;
        public ModuleEvent OnModuleRemoved;

        [System.Serializable]
        public class ConnectionEvent : UnityEvent<Connection> { }
        public ConnectionEvent OnConnectionAdded;
        public ConnectionEvent OnConnectionRemoved;

        [System.Serializable]
        public class CompositionEvent : UnityEvent<Composition> { }
        public CompositionEvent OnCompositionEmpty;

        [System.Serializable]
        public class OriginEvent : UnityEvent<Module, Module> { }
        public OriginEvent OnOriginChanged;

        [SerializeField]
        [HideInInspector]
        private List<Module> modules = new List<Module>();

        [SerializeField]
        [HideInInspector]
        private List<Connection> connections = new List<Connection>();

        public List<Module> Modules
        {
            get
            {
                return modules;
            }
        }

        public List<Connection> Connections
        {
            get
            {
                return connections;
            }
        }

        public bool Connect(Socket connector, Socket conectee)
        {
            Module module = conectee.Module;
            if (!modules.Contains(module))
            {
                AddModule(module);
            }

            if (GetConnection(connector, conectee) != null)
            {
                return false;
            }

            Connection connection = new Connection();
            connection.Connector = connector;
            connection.Connectee = conectee;
            connections.Add(connection);

            OnConnectionAdded.Invoke(connection);
            
            return true;
        }

        public bool Connect(Connection connection)
        {
            return Connect(connection.Connector, connection.Connectee);
        }

        public bool Disconnect(Socket connector, Socket conectee)
        {
            Module module = conectee.Module;

            Connection connection = GetConnection(connector.Module, conectee.Module);
            if (connection == null)
            {
                return false;
            }

            RemoveConnection(connection);

            List<Module> connectedModules = GetConnectedModulesRecursive(conectee.Module);

            if (connectedModules.Count >= 2 && !IsConnected(module, connectedModules))
            {
                Composition newComposition = CreateComposition(module);
                foreach(Module child in connectedModules)
                {
                    List<Connection> childConnections = GetConnections(child);
                    foreach(Connection childConnection in childConnections)
                    {
                        RemoveConnection(childConnection);
                        RemoveModule(childConnection.Connectee.Module);
                        newComposition.Connect(childConnection);
                    }
                }

                RemoveModule(connection.Connectee.Module);
            }

            if (Connections.Count == 0)
            {
                OnCompositionEmpty.Invoke(this);
            }

            return true;
        }

        public bool Disconnect(Connection connection)
        {
            return Disconnect(connection.Connector, connection.Connectee);
        }

        private void RemoveConnection(Connection connection)
        {
            if (!this.connections.Contains(connection))
            {
                return;
            }

            this.connections.Remove(connection);

            OnConnectionRemoved.Invoke(connection);
            
            List<Connection> connections = GetConnections(connection.Connectee.Module);
            if (connections.Count == 0)
            {
                RemoveModule(connection.Connectee.Module);
            }
        }

        public void AddModule(Module module)
        {
            if (!modules.Contains(module))
            {
                modules.Add(module);

                if(module.Composition != this)
                {
                    module.SetComposition(this);
                }

                OnModuleAdded.Invoke(module);
            }
        }

        public void RemoveModule(Module module)
        {
            if(module == Origin)
            {
                SetOrigin(null);
            }

            if (modules.Contains(module))
            {
                if(module.Composition == this)
                {
                    module.SetComposition(null);
                }
                modules.Remove(module);

                OnModuleRemoved.Invoke(module);
               
                if(modules.Count == 0)
                {
                    OnCompositionEmpty.Invoke(this);
                }
            }
        }

        void Awake()
        {
            Module module = GetComponent<Module>();
            if (module == null)
            {
                return;
            }

            SetOrigin(module);
            AddModule(module);
        }

        void OnDrawGizmos()
        {
            SocketItGizmo.DrawComposition(this);
        }

        public List<Module> GetConnectedModulesRecursive(Module startModule, List<Module> modules = null)
        {
            if (modules == null)
            {
                modules = new List<Module>();
            }

            if (!modules.Contains(startModule)){
                modules.Add(startModule);
            }

            List<Module> toCheck = new List<Module>();

            foreach (Connection connection in GetConnections(startModule))
            {
                toCheck.Add(connection.Connectee.Module);
            }

            foreach (Module module in toCheck)
            {
                if (modules.Contains(module))
                {
                    continue;
                }

                GetConnectedModulesRecursive(module, modules);
            }

            return modules;
        }

        private bool IsConnected(Module module, List<Module> modules = null)
        {
            if (module == Origin)
            {
                return true;
            }

            if (modules == null)
            {
                modules = new List<Module>();
            }
            modules.AddRange(GetConnectedModulesRecursive(module));

            foreach (Module toCheck in modules)
            {
                if(toCheck != module && toCheck == Origin)
                {
                    return true;
                }
            } 

            return false;
        }

        public void DisconnectModules(Module module1, Module module2)
        {
            Connection connection = GetConnection(module1, module2);
            Disconnect(connection.Connector, connection.Connectee);
        }

        public List<Connection> GetConnections(Module module)
        {
            List<Connection> connections = new List<Connection>();
            foreach(Connection connection in this.connections)
            {
                if(connection.Connector.Module == module || connection.Connectee.Module == module)
                {
                    connections.Add(connection);
                }
            }

            return connections;
        }

        public Connection GetConnection(Module moduleA, Module moduleB)
        {
            foreach (Connection connection in connections)
            {
                if ((moduleA == connection.Connector.Module && moduleB == connection.Connectee.Module) || 
                    (moduleA == connection.Connectee.Module && moduleB == connection.Connector.Module))
                {
                    return connection;
                }
            }

            return null;
        }

        public Connection GetConnection(Socket socketA, Socket socketB)
        {
            foreach (Connection connection in connections)
            {
                if ((socketA == connection.Connector && socketB == connection.Connectee) ||
                    (socketA == connection.Connectee && socketB == connection.Connector))
                {
                    return connection;
                }
            }

            return null;
        }

        public Connection GetConnection(Socket socket)
        {
            foreach (Connection connection in connections)
            {
                if (socket == connection.Connector || socket == connection.Connectee)
                {
                    return connection;
                }
            }

            return null;
        }

        public static void OnPreConnect(Module connector, Module connectee)
        {
            if (connector.Composition == null && connectee.Composition == null)
            {
                Composition compositon = CreateComposition(connector);
                connector.SetComposition(compositon);
                connectee.SetComposition(compositon);
            }
            else
            {
                if (connector.Composition != null && connectee.Composition == null)
                {
                    connectee.SetComposition(connector.Composition);
                }

                else if (connector.Composition == null && connectee.Composition != null)
                {
                    Composition oldComposition = connectee.Composition;
                    Composition compositon = CreateComposition(connector);
                    connector.SetComposition(compositon);
                    connector.Composition.IntegrateOther(connectee.Composition);
                    connectee.SetComposition(compositon);

                    oldComposition.Clear();

                }
                else if (connector.Composition != null && connectee.Composition != null)
                {
                    Composition oldComposition = connectee.Composition;
                    connector.Composition.IntegrateOther(connectee.Composition);
                    connectee.SetComposition(connector.Composition);
                    oldComposition.Clear();
                }
            }
        }

        private void IntegrateOther(Composition otherComposition)
        {
            foreach(Connection connection in otherComposition.connections)
            {
                if (GetConnection(connection.Connector, connection.Connectee) == null)
                {
                    connections.Add(connection);
                }   
            }

            foreach(Module module in otherComposition.modules)
            {
                AddModule(module);
            }
        }

        public static Composition CreateComposition(Module origin)
        {
            CompositionManager manager = CompositionManager.Instance;
            Composition composition = null;
            composition = manager.CreateComposition();

            if (manager.compositionPrefab == null)
            {
                manager.compositionPrefab = composition;
            }

            composition.SetOrigin(origin);
            composition.AddModule(origin);
            
            return composition;
        }

        public void SetOrigin(Module newOrigin)
        {
            if(newOrigin == Origin)
            {
                return;
            }

            Module oldOrigin = Origin;
            Origin = newOrigin;

            OnOriginChanged.Invoke(newOrigin, oldOrigin);
        }

        public void OnDestroy()
        {
            Clear();
        }

        /**
         * Remove all Modules and Connections from this Composition
         */
        public void Clear()
        {
            connections.Clear();
            foreach (Module module in modules)
            {
                if(module.Composition == this)
                {
                    module.Composition = null;
                }

                if(module == Origin)
                {
                    SetOrigin(null);
                }
            }

            modules.Clear();

            OnCompositionEmpty.Invoke(this);
            
            if (!Application.isPlaying)
            {
                DestroyImmediate(gameObject);
            }
        }
    }
}

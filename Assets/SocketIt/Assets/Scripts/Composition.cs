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
        public List<Connection> Connections = new List<Connection>();

        public Module Origin = null;

        public delegate void ModuleEvent(Module module);
        public delegate void ConnectionEvent(Connection module);

        public event ModuleEvent OnModuleAdded;
        public event ModuleEvent OnModuleRemoved;
        public event ConnectionEvent OnConnectionAdded;
        public event ConnectionEvent OnConnectionRemoved;

        private static int MaxCompositionId = 0;

        public bool Connect(Socket connector, Socket conectee)
        {
            Module module = conectee.Module;
            if (!Modules.Contains(module))
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
            Connections.Add(connection);

            if(OnConnectionAdded != null)
            {
                OnConnectionAdded(connection);
            }

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

            return true;
        }

        private void RemoveConnection(Connection connection)
        {
            if (!Connections.Contains(connection))
            {
                return;
            }

            Connections.Remove(connection);

            if (OnConnectionRemoved != null)
            {
                OnConnectionRemoved(connection);
            }

            List<Connection> connections = GetConnections(connection.Connectee.Module);
            if (connections.Count == 0)
            {
                RemoveModule(connection.Connectee.Module);
            }
        }

        public bool Disconnect(Connection connection)
        {
            return Disconnect(connection.Connector, connection.Connectee);
        }

        public void AddModule(Module module)
        {
            if (!Modules.Contains(module))
            {
                Modules.Add(module);

                if(module.Composition != this)
                {
                    module.SetComposition(this);
                }

                if (OnModuleAdded != null)
                {
                    OnModuleAdded(module);
                }
            }
        }

        public void RemoveModule(Module module)
        {
            if(module == Origin)
            {
                return;
            }

            if (Modules.Contains(module))
            {
                if(module.Composition == this)
                {
                    module.SetComposition(null);
                }
                Modules.Remove(module);

                if (OnModuleRemoved != null)
                {
                    OnModuleRemoved(module);
                }
            }
            
            if (Modules.Count < 2)
            {
                Origin.Composition = null;
                Destroy(gameObject);
            }
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

        internal void DisconnectModules(Module module1, Module module2)
        {
            Connection connection = GetConnection(module1, module2);
            Disconnect(connection.Connector, connection.Connectee);
        }

        public void Delete()
        {
            foreach(Module module in Modules)
            {
                module.Composition = null;
            }

            Destroy(gameObject);
        }

        public List<Connection> GetConnections(Module module)
        {
            List<Connection> connections = new List<Connection>();
            foreach(Connection connection in Connections)
            {
                if(connection.Connector.Module == module || connection.Connectee.Module == this)
                {
                    connections.Add(connection);
                }
            }

            return connections;
        }

        public Connection GetConnection(Module moduleA, Module moduleB)
        {
            foreach (Connection connection in Connections)
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
            foreach (Connection connection in Connections)
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
            foreach (Connection connection in Connections)
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

                    DestroyComposition(oldComposition);

                }
                else if (connector.Composition != null && connectee.Composition != null)
                {
                    Composition oldComposition = connectee.Composition;
                    connector.Composition.IntegrateOther(connectee.Composition);
                    connectee.SetComposition(connector.Composition);
                    DestroyComposition(oldComposition);
                }
            }
        }

        private void IntegrateOther(Composition otherComposition)
        {
            foreach(Connection connection in otherComposition.Connections)
            {
                if (GetConnection(connection.Connector, connection.Connectee) == null)
                {
                    Connections.Add(connection);
                }   
            }

            foreach(Module module in otherComposition.Modules)
            {
                AddModule(module);
            }
        }

        private static Composition CreateComposition(Module origin)
        {
            int newId = ++MaxCompositionId;
            GameObject go = new GameObject("Composition " + newId);
            Composition composition = go.AddComponent<Composition>();
            composition.Origin = origin;
            composition.AddModule(origin);
            return composition;
        }

        public static void DestroyComposition(Composition composition)
        {
            foreach(Module module in composition.Modules){
                if(module.Composition == composition)
                {
                    //module.Composition = null;
                }
            }

            Destroy(composition.gameObject);
        }
    }
}

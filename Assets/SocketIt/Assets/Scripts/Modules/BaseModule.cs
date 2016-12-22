using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SocketIt
{

    public class BaseModule : MonoBehaviour
    {
        private ModuleNode node;
        public List<Module> Modules = new List<Module>();

        public void Start()
        {
            node = GetComponent<ModuleNode>();
            if (node != null)
            {
                node.OnConnectChild += OnConnectChild;
                node.OnDisconnectChild += OnDisconnectChild;

            }
        }

        private void OnDisconnectChild(ModuleNode node)
        {
            List<ModuleNode> affectedNodes = GetAllChilds(node);
            affectedNodes.Add(node);
            
            foreach(ModuleNode affectedNode in affectedNodes)
            {
                Deactivate(affectedNode);
            }

        }

        private void OnConnectChild(ModuleNode node)
        {
            List<ModuleNode> affectedNodes = GetAllChilds(node);
            affectedNodes.Add(node);

            foreach (ModuleNode affectedNode in affectedNodes)
            {
                Activate(affectedNode);
            }
        }

        private List<ModuleNode> GetAllChilds(ModuleNode node)
        {
            List<ModuleNode> childs = new List<ModuleNode>();

            childs = new List<ModuleNode>(node.ChildNodes);

            foreach (ModuleNode child in node.ChildNodes)
            {
                childs.AddRange(GetAllChilds(child));
            }

            return childs;
        }

        private void Activate(ModuleNode node)
        {
            IBaseConnector connector = node.GetComponent<IBaseConnector>();
            if(connector != null)
            {
                connector.Connect(this);
            }
        }

        private void Deactivate(ModuleNode node)
        {
            IBaseConnector connector = node.GetComponent<IBaseConnector>();
            if (connector != null)
            {
                connector.Disconnect(this);
            }
        }
    }   
}

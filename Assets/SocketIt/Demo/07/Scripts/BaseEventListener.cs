﻿using UnityEngine;
using System.Collections;
using SocketIt;
using System;

namespace SocketIt.Example07
{
    public class BaseEventListener : MonoBehaviour
    {
        private BaseModule baseModule;

        public void Awake()
        {
            baseModule = GetComponent<BaseModule>();
            baseModule.OnNodeConnected += OnNodeConnected;
            baseModule.OnNodeConnectedIndirect += OnNodeConnectedIndirect;
            baseModule.OnNodeDisconnected += OnNodeDisconnected;
            baseModule.OnNodeDisconnectedIndirect += OnNodeDisconnectedIndirect;

        }

        private void OnNodeDisconnectedIndirect(ModuleNode node)
        {
            Debug.Log(string.Format(
                "Node {0} disconnected indirect to {1}",
                node.name,
                name
            ));
        }

        private void OnNodeDisconnected(ModuleNode node)
        {
            Debug.Log(string.Format(
                "Node {0} disconnected to {1}",
                node.name,
                name
            ));
        }

        private void OnNodeConnectedIndirect(ModuleNode node)
        {
            Debug.Log(string.Format(
                "Node {0} connected indirect to {1}",
                node.name,
                name
            ));
        }

        private void OnNodeConnected(ModuleNode node)
        {
            Debug.Log(string.Format(
                "Node {0} connected to {1}",
                node.name,
                name
            ));
        }
    }
}

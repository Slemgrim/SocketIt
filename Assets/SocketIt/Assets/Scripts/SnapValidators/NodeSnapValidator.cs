using UnityEngine;
using System.Collections;

namespace SocketIt
{
    public class NodeSnapValidator : MonoBehaviour, ISnapValidator
    {
        public bool ConnectToChildNodes = true;

        public bool Validate(Snap snap)
        {
            ModuleNode node = snap.SocketB.Socket.Module.GetComponent<ModuleNode>();

            if (IsRootNode(node) || IsConnectedToTree(node))
            {
                return true;
            }

            return false;
        }

        private bool IsRootNode(ModuleNode node)
        {
            if (node != false && node.IsRootNode)
            {
                return true;
            }

            return false;
        }

        private bool IsConnectedToTree(ModuleNode node)
        {
            if (ConnectToChildNodes && node != false && node.ParentNode != null)
            {
                return true;
            }

            return false;
        }
    }
}

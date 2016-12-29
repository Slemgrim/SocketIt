using UnityEngine;
using System.Collections;

namespace SocketIt
{
    [RequireComponent(typeof(SnapModule))]
    [RequireComponent(typeof(NodeModule))]
    [AddComponentMenu("SocketIt/Snap Validators/Node Validator")]
    public class NodeSnapValidator : MonoBehaviour, ISnapValidator
    {
        public bool ConnectToChildNodes = true;

        public bool Validate(Snap snap)
        {
            NodeModule node = snap.SocketB.Socket.Module.GetComponent<NodeModule>();

            if (IsRootNode(node) || IsConnectedToTree(node))
            {
                return true;
            }

            return false;
        }

        private bool IsRootNode(NodeModule node)
        {
            if (node != false && node.IsRootNode)
            {
                return true;
            }

            return false;
        }

        private bool IsConnectedToTree(NodeModule node)
        {
            if (ConnectToChildNodes && node != false && node.ParentNode != null)
            {
                return true;
            }

            return false;
        }
    }
}

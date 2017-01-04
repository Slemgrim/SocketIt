using UnityEngine;
using System.Collections.Generic;

namespace SocketIt
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Module))]
    [AddComponentMenu("SocketIt/Module/Snap Module")]
    public class SnapModule : MonoBehaviour
    {
        /**
         * Only non static modules can snap. Uncheck this before you want to snap this module. We need this to prevent
         * both modules snapping at the same time.
         */
        public bool IsStatic = true;
        public bool IsLocked = false;

        private List<SnapSocket> Sockets;

        public delegate void SnapModuleEvent(Snap snap);
        public event SnapModuleEvent OnSnap;

        private List<ISnapValidator> validators;

        public void Awake()
        {
            Sockets = new List<SnapSocket>(GetComponentsInChildren<SnapSocket>());
            validators = new List<ISnapValidator>(GetComponents<ISnapValidator>());
        }

        /**
         * Gets called whenever one of the modules socket finds another socket
         */
        public void OnSocketSnap(SnapSocket callingSocket, SnapSocket otherSocket)
        {
            Snap(callingSocket, otherSocket);
        }

        /**
         * Gets called when one of the modules sockets gets destroyed. 
         */
        public void OnSocketDestroyed(SnapSocket destroyedSocket)
        {
            RemoveSocket(destroyedSocket);
        }

        public void RemoveSocket(SnapSocket socketToRemove)
        {
            if (!Sockets.Contains(socketToRemove))
            {
                return;
            }

            Sockets.Remove(socketToRemove);
        }

        private void Snap(SnapSocket callingSocket, SnapSocket otherSocket)
        {
            Snap snap = new Snap(callingSocket, otherSocket);
            if (IsStatic || !validateSnap(snap))
            {
                return;
            }

            if (OnSnap != null)
            {
                OnSnap(snap);
            }
        }

        private bool validateSnap(Snap snap)
        {
            foreach(ISnapValidator validator in validators)
            {
                if (!validator.Validate(snap))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

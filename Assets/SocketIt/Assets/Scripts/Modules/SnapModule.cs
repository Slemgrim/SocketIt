using UnityEngine;
using System.Collections.Generic;

namespace SocketIt
{
    public class SnapModule : MonoBehaviour
    {
        /**
         * Only non static modules can snap. Uncheck this before you want to snap this module. We need this to prevent
         * both modules snapping at the same time.
         */
        public bool IsStatic = true;
        public bool IsLockedWhenSocketIsLocked = false;
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
        public void OnSocketFound(SnapSocket callingSocket, SnapSocket otherSocket)
        {
            Snap(callingSocket, otherSocket);
        }

        /**
         * Gets called whenever one of the modules socket looses a previous found socket
         */
        public void OnSocketLost(SnapSocket callingSocket, SnapSocket otherSocket)
        {
            return;
        }

        /**
         * Gets called when one of the modules sockets gets destroyed. 
         */
        public void OnSocketDestroyed(SnapSocket destroyedSocket)
        {
            RemoveSocket(destroyedSocket);
        }

        /**
         * Gets called when a socked gets Looked. We use this to also look the module if
         * the IsLockedWhenSocketIsLocked property is checked
         */
        public void OnSocketLock(SnapSocket lockedSocket)
        {
            if (!IsLockedWhenSocketIsLocked)
            {
                return;
            }

            foreach (SnapSocket socket in Sockets)
            {
                if (socket.IsLocked)
                {
                    IsLocked = true;
                }
            }
        }

        public void OnSocketUnlock(SnapSocket lockedSocket)
        {
            if (!IsLockedWhenSocketIsLocked)
            {
                return;
            }

            foreach (SnapSocket socket in Sockets)
            {
                if (socket.IsLocked)
                {
                    return;
                }
            }

            IsLocked = false;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocketIt
{
    [System.Serializable]
    public class Snap
    {
        public Socket SocketA;
        public Socket SocketB;

        public Snap(Socket socketA, Socket socketB)
        {
            SocketA = socketA;
            SocketB = socketB;
        }
    }
}

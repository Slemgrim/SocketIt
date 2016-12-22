using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocketIt
{
    [System.Serializable]
    public class Snap
    {
        public SnapSocket SocketA;
        public SnapSocket SocketB;

        public Snap(SnapSocket socketA, SnapSocket socketB)
        {
            SocketA = socketA;
            SocketB = socketB;
        }
    }
}

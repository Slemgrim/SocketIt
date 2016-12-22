using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocketIt {
    [System.Serializable]
    public class Connection
    {
        public Socket SocketA;
        public Socket SocketB;

        public Connection(Socket socketA, Socket socketB)
        {
            SocketA = socketA;
            SocketB = socketB;
        }
    }
}

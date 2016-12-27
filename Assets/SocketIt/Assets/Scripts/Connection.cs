using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocketIt {
    [System.Serializable]
    public class Connection
    {
        public Socket SocketA;
        public Socket SocketB;
        public Socket Initiator;

        public Connection(Socket socketA, Socket socketB, Socket initiator)
        {
            SocketA = socketA;
            SocketB = socketB;
            Initiator = initiator;
        }
    }
}

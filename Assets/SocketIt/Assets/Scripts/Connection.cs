using UnityEngine;

namespace SocketIt {
    [System.Serializable]
    public class Connection
    {
        public Socket Connector;
        public Socket Connectee;

        public bool ContainsSocket(Socket socket)
        {
            return Connectee == socket || Connector == socket;
        }
    }


}

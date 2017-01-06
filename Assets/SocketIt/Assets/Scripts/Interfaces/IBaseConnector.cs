using UnityEngine;
using System.Collections;

namespace SocketIt
{
    public interface IBaseConnector
    {
        void Connect(MasterModule baseModule);
        void Disconnect(MasterModule baseModule);
    }
}
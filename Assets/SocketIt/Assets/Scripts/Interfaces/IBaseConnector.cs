using UnityEngine;
using System.Collections;

namespace SocketIt
{
    public interface IBaseConnector
    {
        void Connect(BaseModule baseModule);
        void Disconnect(BaseModule baseModule);
    }
}
using UnityEngine;
using System.Collections;

namespace SocketIt {
	public delegate void ModuleConnectEvent(Connection connection);

	public interface IModuleConnector
	{
		event ModuleConnectEvent OnConnectStart;
        event ModuleConnectEvent OnConnectEnd;

        event ModuleConnectEvent OnDisconnectStart;
        event ModuleConnectEvent OnDisconnectEnd;
    }
}
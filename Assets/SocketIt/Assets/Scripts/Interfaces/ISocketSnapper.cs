using UnityEngine;
using System.Collections;

namespace SocketIt {
	public delegate void SnapEvent(SnapSocket ownSocket, SnapSocket otherSocket);

	public interface ISocketSnapper
	{
        event SnapEvent OnSnapStart;
        event SnapEvent OnSnapEnd;
        void StartSnapping(Snap snap);
        void StopSnapping();
	}
}
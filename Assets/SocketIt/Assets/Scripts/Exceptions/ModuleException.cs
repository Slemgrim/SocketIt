using System;

namespace SocketIt {
	public class ModuleException : SocketItException {
		public ModuleException(string message): base(message)
		{
		}
	}
}

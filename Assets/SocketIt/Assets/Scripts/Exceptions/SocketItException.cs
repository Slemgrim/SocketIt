using System;

namespace SocketIt
{
    public class SocketItException : Exception
    {
        public SocketItException(string message) : base(message)
        {
        }
    }
}

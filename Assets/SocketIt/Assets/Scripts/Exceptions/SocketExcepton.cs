using System;

namespace SocketIt
{
    public class SocketException : SocketItException
    {
        public SocketException(string message) : base(message)
        {
        }
    }
}

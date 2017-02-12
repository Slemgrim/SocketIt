using System;

namespace SocketIt
{
    /// <summary>
    /// Generic exception thrown by Socket related classes
    /// </summary>
    /// <remarks>
    /// SocketException extends SocketItException in order to catch all SocketIt Exceptions at once
    /// </remarks>
    public class SocketException : SocketItException
    {
        public SocketException(string message) : base(message)
        {
        }
    }
}

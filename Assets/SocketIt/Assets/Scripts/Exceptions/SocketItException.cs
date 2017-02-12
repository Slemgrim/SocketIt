using System;

namespace SocketIt
{
    /// <summary>
    /// Generic exception thrown by SocketIt
    /// </summary>
    /// <remarks>
    /// All exceptions thrown by SocketIt are based on this
    /// </remarks>
    public class SocketItException : Exception
    {
        public SocketItException(string message) : base(message)
        {
        }
    }
}

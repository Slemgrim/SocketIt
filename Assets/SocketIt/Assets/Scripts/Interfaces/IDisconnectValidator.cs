namespace SocketIt
{

    /// <summary>
    /// Interface for dissconnection validation. Add an implementation of this interface 
    /// to the Composition.DisconnectValidators list and controll whether a disconnection request between
    /// two Sockets should be allowed or discarded
    /// </summary>
    public interface IDisconnectValidator
    {

        /// <summary>
        /// Gets called by every Composition that uses this IDisconnectValidator
        /// </summary>
        /// <seealso cref="IConnectValidator">
        /// <param name="connector">The Socket which requests a disconnection from <paramref name="conectee"/></param>
        /// <param name="connectee">The Socket whicht receives a disconnection request from <paramref name="connector"/> </param>
        /// <returns>True or falls whether the dissconnect should be allowed or discarded</returns>
        bool Validate(Socket connector, Socket connectee);
    }
}
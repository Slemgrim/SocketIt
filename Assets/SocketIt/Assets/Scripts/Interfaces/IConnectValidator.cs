namespace SocketIt
{

    /// <summary>
    /// Interface for connection validation. Add an implementation of this interface 
    /// to the Composition.ConnectValidators list and controll whether a connection to that Composition will be allowed or discarded
    /// </summary>
    public interface IConnectValidator
    {

        /// <summary>
        /// Gets called by every Composition that uses this IConnectValidator
        /// </summary>
        /// <seealso cref="IDisconnectValidator">
        /// <param name="connector">The Socket which requests a Connection to <paramref name="conectee"/></param>
        /// <param name="connectee">The Socket whicht receives a Connection request from <paramref name="connector"/> </param>
        /// <returns>True or falls whether the connection should be allowed or discarded</returns>
        bool Validate(Socket connector, Socket connectee);
    }
}
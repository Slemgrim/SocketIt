namespace SocketIt {

    /// <summary>
    /// Interface for Snap validation. Add an implementation of this interface 
    /// to the Module.SnapValidators list and controll whether a Snap to that Module should be allowed or discarded
    /// </summary>
    public interface ISnapValidator
	{

        /// <summary>
        /// Gets called by every Module that uses an implementation of ISnapValidator
        /// </summary>
        /// <param name="snap">The Snap request between two Sockets</param>
        /// <returns>True or falls whether the Snap should be allowed or discarded</returns>
        bool Validate(Snap snap);
	}
}
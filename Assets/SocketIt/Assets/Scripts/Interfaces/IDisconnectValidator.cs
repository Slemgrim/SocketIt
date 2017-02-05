namespace SocketIt
{
    public interface IDisconnectValidator
    {
        bool Validate(Socket connector, Socket connectee);
    }
}
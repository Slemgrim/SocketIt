namespace SocketIt
{
    public interface IConnectValidator
    {
        bool Validate(Socket connector, Socket connectee);
    }
}
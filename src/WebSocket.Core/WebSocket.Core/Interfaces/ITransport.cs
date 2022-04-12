namespace WebSocket.Core.Interfaces;

public interface ITransport: IDisposable
{
    event EventHandler<string> Message;
    event EventHandler Disconnected;
        
    Task Connect(CancellationToken token);
    Task Disconnect();

    Task Send(string data, CancellationToken token);
}
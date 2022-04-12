namespace WebSocket.Core.Interfaces;

public interface ITransportFactory
{
    string Name { get; }
    bool Enabled { get; set; }
    uint Priority { get; set; }

    Task<ITransport> Build(ITransportConfiguration config);
}
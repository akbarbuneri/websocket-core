
using System.Diagnostics.CodeAnalysis;
using WebSocket.Core.Enums;

namespace WebSocket.Core.Interfaces;

public interface IClient: IDisposable
{
    event EventHandler Connected;
    event EventHandler Disconnected;
    event EventHandler<string> Message;

    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    ConnectionState State { get; }

    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    Task Connect(CancellationToken token);
    Task Connect();
    Task Disconnect();

    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    Task Send(string data, CancellationToken token);
    Task Send(string data);
}
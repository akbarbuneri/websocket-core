namespace WebSocket.Core.Interfaces;

public interface ILogger
{
    void Debug(string message);
    void Info(string message);
    void Error(string message);
}
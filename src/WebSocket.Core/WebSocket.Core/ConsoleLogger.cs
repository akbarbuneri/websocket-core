using System.Diagnostics;
using WebSocket.Core.Interfaces;

namespace WebSocket.Core;

public class ConsoleLogger: ILogger
{
    [DebuggerStepThrough, DebuggerNonUserCode]
    public void Debug(string message) => Console.WriteLine($"{DateTime.Now:s} [DBG] {message}");

    [DebuggerStepThrough, DebuggerNonUserCode]
    public void Info(string message) => Console.WriteLine($"{DateTime.Now:s} [INF] {message}");

    [DebuggerStepThrough, DebuggerNonUserCode]
    public void Error(string message) => Console.WriteLine($"{DateTime.Now:s} [ERR] {message}");
}
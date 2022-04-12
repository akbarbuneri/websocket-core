using System.ComponentModel;

namespace WebSocket.Core.Enums;

public enum MessageType
{
    [Description("o")] Open,
    [Description("h")] Heartbeat,
    [Description("a")] ArrayMessage,
    [Description("m")] SingleMessage,
    [Description("c")] Close,
    Unknown
}
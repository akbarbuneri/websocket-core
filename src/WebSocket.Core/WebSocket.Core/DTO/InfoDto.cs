#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
using Newtonsoft.Json;

namespace WebSocket.Core.DTO;

public class InfoDto
{
    [JsonProperty(PropertyName = "entropy")]
    public long Entropy { get; set; }

    [JsonProperty(PropertyName = "origins")]
    public string[] Origins { get; set; }

    [JsonProperty(PropertyName = "cookie_needed")]
    public bool CookieNeeded { get; set; }

    [JsonProperty(PropertyName = "websocket")]
    public bool WebSocket { get; set; }

    [JsonIgnore]
    public long RoundTripTime { get; set; }
}
using System.Diagnostics;
using Newtonsoft.Json;
using WebSocket.Core.DTO;
using WebSocket.Core.Interfaces;

namespace WebSocket.Core;

internal class InfoReceiver
{
    private readonly Configuration.Factory.ReadOnlySockJsConfiguration _config;
    private readonly ILogger _log;

    public InfoReceiver(Configuration.Factory.ReadOnlySockJsConfiguration config)
    {
        _config = config;
        _log = config.Logger;
    }

    public async Task<InfoDto> GetInfo()
    {
        _log.Debug($"{nameof(GetInfo)}: Base URL: {_config.BaseEndpoint}");
        await Task.Delay(0);
        var baseUri = _config.BaseEndpoint;
        var url = new Uri(baseUri, $"{baseUri.AbsolutePath}/info?t={DateTimeOffset.Now.ToUnixTimeMilliseconds()}").OriginalString;
        _log.Debug($"{nameof(GetInfo)}: Info URL: {url}");

        using var client = new HttpClient();
        using var cts = new CancellationTokenSource(_config.InfoReceiverTimeout);

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        _config.DefaultHeaders?
            .OfType<string>()
            .Select(header => (header, value: _config.DefaultHeaders[header]))
            .ForEach(i => request.Headers.Add(i.header, i.value));

        var stopwatch = Stopwatch.StartNew();
        var response = await client.SendAsync(request, cts.Token);
        response.EnsureSuccessStatusCode();
        stopwatch.Stop();

        var content = await response.Content.ReadAsStringAsync();
        var info = JsonConvert.DeserializeObject<InfoDto>(content);
#pragma warning disable CS8603
        if (info == null) return info;
#pragma warning restore CS8603
        info.RoundTripTime = stopwatch.ElapsedMilliseconds;

        _log.Debug($"{nameof(GetInfo)}: Finish {content} {info.RoundTripTime}");
        return info;
    }
}
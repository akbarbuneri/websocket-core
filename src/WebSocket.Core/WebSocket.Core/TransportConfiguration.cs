using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using WebSocket.Core.DTO;
using WebSocket.Core.Interfaces;

namespace WebSocket.Core;

internal class TransportConfiguration : ITransportConfiguration
{
    private readonly Configuration.Factory.ReadOnlySockJsConfiguration _config;

    public TransportConfiguration(Configuration.Factory.ReadOnlySockJsConfiguration config, InfoDto info)
    {
        Info = info;
        _config = config;
    }

    #region Implementation of ITransportConfiguration
    public Uri BaseEndpoint => _config.BaseEndpoint;
    public ILogger Logger => _config.Logger;
    public InfoDto Info { get; }
    public WebHeaderCollection DefaultRequestHeaders => _config.DefaultHeaders;
    public ICredentials? Credentials => _config.Credentials;
    public IWebProxy? Proxy => _config.Proxy;
    public X509CertificateCollection? ClientCertificates => _config.ClientCertificates;
    public RemoteCertificateValidationCallback? RemoteCertificateValidator => _config.RemoteCertificateValidator;
    public CookieContainer? Cookies => _config.Cookies;
    public TimeSpan? KeepAliveInterval => _config.KeepAliveInterval;
    #endregion Implementation of ITransportConfiguration
}
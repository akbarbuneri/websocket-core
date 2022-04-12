using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using WebSocket.Core.DTO;

namespace WebSocket.Core.Interfaces;

public interface ITransportConfiguration
{
    Uri BaseEndpoint { get; }
    ILogger Logger { get; }
    InfoDto Info { get; }
    WebHeaderCollection DefaultRequestHeaders { get; }
    ICredentials? Credentials { get; }
    IWebProxy? Proxy { get; }
    X509CertificateCollection? ClientCertificates { get; }
    RemoteCertificateValidationCallback? RemoteCertificateValidator { get; }
    CookieContainer? Cookies { get; }
    TimeSpan? KeepAliveInterval { get; }
}
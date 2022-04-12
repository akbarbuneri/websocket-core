using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using WebSocket.Core.Interfaces;

namespace WebSocket.Core;

  public class Configuration
    {
        public ICollection<ITransportFactory>? TransportFactories { get; set; }
        public Uri? BaseEndpoint { get; set; }
        public WebHeaderCollection? DefaultHeaders { get; set; }
        public ILogger? Logger { get; set; }
        public TimeSpan? InfoReceiverTimeout { get; set; }
        public ICredentials? Credentials { get; set; }
        public IWebProxy? Proxy { get; set; }
        public X509CertificateCollection? ClientCertificates { get; set; }
        public RemoteCertificateValidationCallback? RemoteCertificateValidator { get; set; }
        public CookieContainer? Cookies { get; set; }
        public TimeSpan? KeepAliveInterval { get; set; }

        internal Factory.ReadOnlySockJsConfiguration AsReadonly() => new Factory.ReadOnlySockJsConfiguration(this);

        public static class Factory
        {
            public static Configuration BuildDefault(string baseEndpoint)
            {
                return BuildDefault(new Uri(baseEndpoint ?? throw new ArgumentNullException(nameof(baseEndpoint))));
            }

            public static Configuration BuildDefault(Uri baseEndpoint)
            {
                if (baseEndpoint is null) throw new ArgumentNullException(nameof(baseEndpoint));

                return new Configuration
                {
                    BaseEndpoint = baseEndpoint,
                    TransportFactories = ReflectTransportFactories(),
                    DefaultHeaders = new WebHeaderCollection(),
                    Logger = new NullLogger(),
                    InfoReceiverTimeout = TimeSpan.FromSeconds(8),
                    Credentials = null,
                    Proxy = null,
                    ClientCertificates = null,
                    RemoteCertificateValidator = null,
                    Cookies = null,
                    KeepAliveInterval = null,
                };
            }

            private static ICollection<ITransportFactory> ReflectTransportFactories()
            {
                var factories = typeof(WebsocketClient)
                    .Assembly
                    .DefinedTypes
                    .Where(t => !t.IsAbstract)
                    .Where(t => typeof(ITransportFactory).IsAssignableFrom(t))
                    .Select(Activator.CreateInstance)
                    .OfType<ITransportFactory>()
                    .OrderByDescending(f => f.Priority)
                    .ToArray();
                return factories;
            }

            internal class ReadOnlySockJsConfiguration
            {
                public ReadOnlySockJsConfiguration(Configuration config)
                {
                    TransportFactories = config.TransportFactories ?? ReflectTransportFactories();
                    BaseEndpoint = config.BaseEndpoint ?? throw new ArgumentNullException(nameof(Configuration.BaseEndpoint));
                    DefaultHeaders = config.DefaultHeaders ?? new WebHeaderCollection();
                    Logger = config.Logger ?? new NullLogger();
                    InfoReceiverTimeout = config.InfoReceiverTimeout ?? TimeSpan.FromSeconds(8);
                    Credentials = config.Credentials;
                    Proxy = config.Proxy;
                    ClientCertificates = config.ClientCertificates;
                    RemoteCertificateValidator = config.RemoteCertificateValidator;
                    Cookies = config.Cookies;
                    KeepAliveInterval = config.KeepAliveInterval;
                }

                public ICollection<ITransportFactory> TransportFactories { get; }
                public Uri BaseEndpoint { get; }
                public WebHeaderCollection DefaultHeaders { get; }
                public ILogger Logger { get; }
                public TimeSpan InfoReceiverTimeout { get; }
                public ICredentials? Credentials { get; }
                public IWebProxy? Proxy { get; }
                public X509CertificateCollection? ClientCertificates { get; }
                public RemoteCertificateValidationCallback? RemoteCertificateValidator { get; }
                public CookieContainer? Cookies { get; }
                public TimeSpan? KeepAliveInterval { get; }
            }
        }
    }
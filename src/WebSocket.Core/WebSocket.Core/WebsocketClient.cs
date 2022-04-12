using System.Diagnostics.CodeAnalysis;
using WebSocket.Core.DTO;
using WebSocket.Core.Enums;
using WebSocket.Core.Interfaces;

namespace WebSocket.Core;

[SuppressMessage("ReSharper", "InconsistentNaming")]
 public class WebsocketClient : IClient
    {
        private readonly SemaphoreSlim _sync = new SemaphoreSlim(1, 1);
        private readonly Configuration.Factory.ReadOnlySockJsConfiguration _config;
        private readonly ILogger _log;
        private ITransport? _transport;
        private ConnectionState _state = ConnectionState.Initial;

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public WebsocketClient(string baseEndpoint) : this(Configuration.Factory.BuildDefault(baseEndpoint)) {}
        
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public WebsocketClient(Uri baseEndpoint) : this(Configuration.Factory.BuildDefault(baseEndpoint)) {}

        public WebsocketClient(Configuration config)
        {
            _config = config?.AsReadonly() ?? throw new ArgumentNullException(nameof(config));
            _log = _config.Logger;
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion Implementation of IDisposable

        #region Implementation of IClient
        public event EventHandler? Connected;
        public event EventHandler? Disconnected;
        public event EventHandler<string>? Message;

        public ConnectionState State
        {
            get => _state;
            private set
            {
                var current = _state;
                if (current == value) return;
                _log.Debug($"{nameof(State)}: {current} -> {value}");
                _state = value;
            }
        }

        public async Task Connect(CancellationToken token)
        {
            _log.Info(nameof(Connect));
            try
            {
                await _sync.WaitAsync(token);
                if (State != ConnectionState.Initial) throw new Exception($"Cannot connect while state is '{State}'");
                State = ConnectionState.Connecting;

                var info = await new InfoReceiver(_config).GetInfo();

                ITransport? selectedTransport = null;
                var factories = _config.TransportFactories.Where(t => t.Enabled).ToArray();
                _log.Debug($"{nameof(Connect)}: Transports: {factories.Length}/{_config.TransportFactories.Count} (enabled/total)");

                foreach (var factory in factories)
                {
                    selectedTransport = await TryTransport(factory, info, token);
                    if (selectedTransport is null) continue;
                    break;
                }

                this._transport = selectedTransport ?? throw new Exception("No available transports");
                _transport.Message += TransportOnMessage;
                _transport.Disconnected += TransportOnDisconnected;
                State = ConnectionState.Established;
                Connected?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                _log.Error($"{nameof(Connect)}: {e.Message}");
                _log.Debug($"{nameof(Connect)}: {e}");
                State = ConnectionState.Error;
                throw;
            }
            finally
            {
                _sync.Release();
            }
        }

        public Task Connect() => Connect(CancellationToken.None);

        public async Task Disconnect()
        {
            _log.Info(nameof(Disconnect));
            VerifyEstablished();
            await _transport!.Disconnect();
        }

        public Task Send(string data) => Send(data, CancellationToken.None);

        public async Task Send(string data, CancellationToken token)
        {
            VerifyEstablished();
            await _transport!.Send(data, token);
        }

        #endregion Implementation of IClient

        private async Task<ITransport?> TryTransport(
            ITransportFactory factory,
            InfoDto info,
            CancellationToken token)
        {
            try
            {
                _log.Debug($"{nameof(TryTransport)}: {factory.Name}");
                var transport = await factory.Build(new TransportConfiguration(_config, info));
                await transport.Connect(token);
                _log.Info($"{nameof(TryTransport)}: {factory.Name} - Success");
                return transport;
            }
            catch (Exception e)
            {
                _log.Error($"{nameof(TryTransport)}: {factory.Name} - Failed: {e.Message}");
                _log.Error($"{nameof(TryTransport)}: {e}");
                return null;
            }
        }

        private void TransportOnMessage(object? sender, string message)
        {
            _log.Debug($"{nameof(TransportOnMessage)}: {message}");
            Message?.Invoke(this, message);
        }

        private void TransportOnDisconnected(object? sender, EventArgs e)
        {
            _log.Debug($"{nameof(TransportOnDisconnected)}");
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        private void VerifyEstablished()
        {
            if (_transport is null || State != ConnectionState.Established)
                throw new InvalidOperationException("Connection not established");
        }
    }
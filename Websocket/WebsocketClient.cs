using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.API;
using YellowMacaroni.Discord.Core;
using YellowMacaroni.Discord.Websocket.Events;

namespace YellowMacaroni.Discord.Websocket
{
    internal class WebsocketClient(Client client, string token, Intents intents)
    {
        private ClientWebSocket _ws = new();
        private readonly Client _parentClient = client;
        private readonly string _token = token;
        private readonly Intents _intents = intents;
        private int _sequence = -1;
        private string _sessionId = String.Empty;
        private int _heartbeatInterval = 41250; // Default heartbeat interval
        private CancellationTokenSource _cts = new();
        private Task? _heartbeatTask = null;
        private Task? _receiveTask = null;
        private int _connectionAttempt = 0;
        private readonly int _connectionAttemptDuration = 5000;
        private string _initialGatewayUrl = String.Empty;
        private string _gatewayUrl = "wss://gateway.discord.gg/?v=10&encoding=json";
        private bool _heartbeatAcked = true;
        private double _lastPingTicks = DateTimeOffset.UtcNow.Ticks; // -1 if no ping, otherwise last ping timestamp as ticks

        public float ping = -1;
        public bool dead = false;

        public event EventHandler<JObject>? MessageRecieved;
        public event EventHandler<JObject>? Dispatch;
        public event EventHandler<Exception>? ErrorOccurred;
        public event EventHandler? Disconnected;
        public event EventHandler? Connecting;
        public event EventHandler? Connected;
        public event EventHandler? Ready;
        public event EventHandler<ClientDebug>? Debug;

        private readonly List<int> canReconenctCodes = [4000, 4001, 4002, 4003, 4005, 4007, 4008, 4009];

        public async Task ConnectAsync(string? gatewayUrl = null)
        {
            try
            {
                if (_ws.State == WebSocketState.Connecting || _ws.State == WebSocketState.Open)
                {
                    Debug?.Invoke(this, new ClientDebug("Refusing to connect websocket because a connection is already open", ClientDebugType.Warn));
                    return;
                }

                if (_connectionAttempt > 3)
                {
                    throw new TooManyAttemptsException($"Websocket failed to connect after {_connectionAttempt} attempts over {(_connectionAttemptDuration / 2000) * ((_connectionAttempt * _connectionAttempt) + _connectionAttempt)} seconds");
                }

                Thread.Sleep(_connectionAttempt * _connectionAttemptDuration);

                Connecting?.Invoke(this, EventArgs.Empty);

                if (gatewayUrl is null)
                {
                    Debug?.Invoke(this, new ClientDebug($"Fetching gateway URI", ClientDebugType.Info));
                    HttpResponseMessage result = await APIHandler.GET("/gateway");
                    if (!result.IsSuccessStatusCode)
                    {
                        throw new Exception($"Failed to get gateway URL: {result.ReasonPhrase}");
                    }
                    JObject payload = JObject.Parse(await result.Content.ReadAsStringAsync());
                    _gatewayUrl = payload["url"]?.ToString() ?? _initialGatewayUrl;
                }
                else _gatewayUrl = gatewayUrl;

                if (_initialGatewayUrl == String.Empty) _initialGatewayUrl = _gatewayUrl;

                Debug?.Invoke(this, new ClientDebug($"Connecting to gateway using URI {_gatewayUrl}", ClientDebugType.Info));

                _ws.Dispose();
                _cts.Cancel();
                _cts.Dispose();

                _ws = new();
                _cts = new();

                if (_receiveTask is not null && _receiveTask.IsCompleted)
                {
                    await Task.WhenAny(_receiveTask, Task.Delay(5000));
                    _receiveTask = null;
                }

                await _ws.ConnectAsync(
                    new Uri(_gatewayUrl),
                    _cts.Token);

                Connected?.Invoke(this, EventArgs.Empty);
                Debug?.Invoke(this, new ClientDebug($"Connected to gateway using URI {_gatewayUrl}", ClientDebugType.Info));

                _connectionAttempt = 0;

                _receiveTask = ReceiveMessagesAsync();
            }
            catch (TooManyAttemptsException ex) {
                ErrorOccurred?.Invoke(this, ex);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, ex);
                _connectionAttempt++;
                Debug?.Invoke(this, new ClientDebug($"Reconnecting to gateway after error, reconnect attempt {_connectionAttempt}", ClientDebugType.Info));
                await ConnectAsync(gatewayUrl);
            }
        }

        public async Task Resume()
        {
            Debug?.Invoke(this, new ClientDebug($"Resume command recieved", ClientDebugType.Info));

            await ConnectAsync(_gatewayUrl);

            await SendJsonAsync(new
            {
                op = 6,
                d = new
                {
                    token = _token,
                    session_id = _sessionId,
                    seq = _sequence
                }
            });

            Debug?.Invoke(this, new ClientDebug($"Resume payload sent using seq {_sequence} and sessionId {_sessionId}", ClientDebugType.Info));
        }

        public async Task DisconnectAsync(bool reconnecting = true)
        {
            if (_ws.State != WebSocketState.Open)
            {
                Debug?.Invoke(this, new ClientDebug("Refusing to disconnect websocket because connection is not open", ClientDebugType.Warn));
                return;
            }

            Debug?.Invoke(this, new ClientDebug($"Disconnecting from gateway and {(reconnecting ? "" : "not ")}reconnecting", ClientDebugType.Info));

            _parentClient.ready = false;

            _cts.Cancel();

            await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnecting", CancellationToken.None);

            _heartbeatTask = null;
            _receiveTask = null;
            _cts = new CancellationTokenSource();

            if (!reconnecting) { dead = true; }
        }

        private async Task ReceiveMessagesAsync()
        {
            byte[]? buffer = new byte[8192];
            StringBuilder messageBuffer = new();

            try
            {
                while (_ws.State == WebSocketState.Open && !_cts.Token.IsCancellationRequested)
                {
                    WebSocketReceiveResult result;
                    do
                    {
                        result = await _ws.ReceiveAsync(
                            new ArraySegment<byte>(buffer), _cts.Token);

                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            messageBuffer.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));
                        }
                    }
                    while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Text && messageBuffer.Length > 0)
                    {
                        _ = Task.Run(async () =>
                        {
                            string? message = messageBuffer.ToString();
                            messageBuffer.Clear();

                            await HandleMessageAsync(message);
                        });                        
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _ws.CloseOutputAsync(
                            WebSocketCloseStatus.NormalClosure,
                            "Server closed connection",
                            CancellationToken.None);

                        _cts.Cancel();

                        if (_ws.CloseStatus.HasValue && canReconenctCodes.Contains((int)_ws.CloseStatus))
                        {
                            await Resume();
                        } else await ConnectAsync(_initialGatewayUrl);

                        Disconnected?.Invoke(this, EventArgs.Empty);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, ex);
                await DisconnectAsync();
                await ConnectAsync();
            }
        }

        private async Task HandleMessageAsync(string message)
        {
            var payload = JObject.Parse(message);
            int op = payload["op"]?.Value<int>() ?? -1;

            // Update sequence if present
            if (payload.TryGetValue("s", out var s) && s.Type != JTokenType.Null)
            {
                _sequence = s.Value<int>();
            }

            switch (op)
            {
                case 0: // Dispatch
                    await HandleDispatch(payload);
                    Dispatch?.Invoke(this, payload);
                    break;

                case 1: // Heartbeat request
                    Debug?.Invoke(this, new ClientDebug($"Heartbeat requested", ClientDebugType.Info));
                    await SendHeartbeatAsync();
                    break;

                case 10: // Hello
                    Debug?.Invoke(this, new ClientDebug($"Hello recieved", ClientDebugType.Info));
                    _heartbeatInterval = payload["d"]?["heartbeat_interval"]?.Value<int>() ?? 41250;
                    _heartbeatTask = HeartbeatLoopAsync();
                    await SendIdentifyAsync();
                    break;

                case 11: // Heartbeat ACK
                    Debug?.Invoke(this, new ClientDebug($"Heartbeat recieved", ClientDebugType.Info));
                    ping = (float)(DateTime.UtcNow.Ticks - _lastPingTicks) / 20000f;
                    _parentClient.ping = ping;
                    _heartbeatAcked = true;
                    break;

                case 7: // Reconnect
                    Debug?.Invoke(this, new ClientDebug($"Reconnect requested", ClientDebugType.Info));
                    await DisconnectAsync();
                    await Resume();
                    break;

                case 9: // Invalid Session
                    if (payload["d"]?.Value<bool>() == true)
                    {
                        await DisconnectAsync(true);
                        await Resume();
                    }
                    else
                    {
                        await DisconnectAsync(true);
                        await ConnectAsync(_initialGatewayUrl);
                    }
                    break;
            }

            MessageRecieved?.Invoke(this, payload);
        }

        private async Task HandleDispatch(JObject payload)
        {
            string thisEventName = payload["t"]?.ToString() ?? string.Empty;
            switch (thisEventName)
            {
                case "READY":
                    _sessionId = payload["d"]?["session_id"]?.ToString() ?? string.Empty;
                    _gatewayUrl = payload["d"]?["resume_gateway_url"]?.ToString() ?? _gatewayUrl;
                    _parentClient.ready = true;
                    Ready?.Invoke(this, EventArgs.Empty);

                    Ready? r = payload["d"]?.ToObject<Ready>();
                    if (r is not null) _parentClient.readyData = r;

                    _parentClient.startupTime = DateTimeOffset.UtcNow;

                    break;
            }
        }

        private async Task HeartbeatLoopAsync()
        {
            try
            {
                while (_ws.State == WebSocketState.Open && !_cts.Token.IsCancellationRequested)
                {
                    if (!_heartbeatAcked)
                    {
                        await DisconnectAsync();
                        await ConnectAsync();
                        return;
                    }

                    _heartbeatAcked = false;
                    _lastPingTicks = DateTimeOffset.UtcNow.UtcTicks;
                    await SendHeartbeatAsync();
                    await Task.Delay(_heartbeatInterval, _cts.Token);
                }
            }
            catch (TaskCanceledException) { /* Expected on disconnect */ }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, ex);
                await DisconnectAsync(true);
                await ConnectAsync();
            }
        }

        private async Task SendHeartbeatAsync()
        {
            Debug?.Invoke(this, new ClientDebug($"Sending heartbeat", ClientDebugType.Info));

            var payload = new
            {
                op = 1,
                d = _sequence == -1 ? (int?)null : _sequence
            };

            await SendJsonAsync(payload);
        }

        private async Task SendIdentifyAsync()
        {
            Debug?.Invoke(this, new ClientDebug($"Sending identify", ClientDebugType.Info));

            var payload = new
            {
                op = 2,
                d = new
                {
                    token = _token,
                    intents = (int)_intents,
                    properties = new
                    {
                        os = Environment.OSVersion.Platform.ToString(),
                        browser = "YellowMacaroni.Discord",
                        device = "YellowMacaroni.Discord"
                    },
                    shard = _parentClient.shard is not null ? new List<int> { _parentClient.shard.id, _parentClient.shard.totalShards } : null
                }
            };

            await SendJsonAsync(payload);
        }

        public async Task SendJsonAsync(object data)
        {
            if (_ws.State != WebSocketState.Open && _ws.State != WebSocketState.CloseReceived) return;

            string? json = JsonConvert.SerializeObject(data);
            byte[]? bytes = Encoding.UTF8.GetBytes(json);

            await _ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, _cts.Token);
        }
    }

    public class TooManyAttemptsException(string? message): Exception(message) { }
}

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

namespace YellowMacaroni.Discord.Websocket
{
    internal class WebsocketClient(Client client, string token, Intents intents)
    {
        private readonly ClientWebSocket _ws = new();
        private readonly Client _parentClient = client;
        private readonly string _token = token;
        private readonly Intents _intents = intents;
        private int _sequence = -1;
        private string _sessionId = String.Empty;
        private int _heartbeatInterval = 41250; // Default heartbeat interval
        private CancellationTokenSource _cts = new();
        private Task? _heartbeatTask = null;
        private Task? _receiveTask = null;
        private string _initalGatewayUrl = String.Empty;
        private string _gatewayUrl = "wss://gateway.discord.gg/?v=10&encoding=json";
        private bool _heartbeatAcked = true;
        private double _lastPingTicks = DateTimeOffset.UtcNow.Ticks; // -1 if no ping, otherwise last ping timestamp as ticks

        public float ping = -1;
        public bool dead = false;

        public event EventHandler<JObject>? MessageRecieved;
        public event EventHandler<JObject>? Dispatch;
        public event EventHandler<Exception>? ErrorOccurred;
        public event EventHandler? Disconnected;
        public event EventHandler? Connected;
        public event EventHandler? Ready;

        private readonly List<int> canReconenctCodes = [4000, 4001, 4002, 4003, 4005, 4007, 4008, 4009];

        public async Task ConnectAsync(string? gatewayUrl = null)
        {
            try
            {
                if (gatewayUrl is null)
                {
                    HttpResponseMessage result = await APIHandler.GET("/gateway");
                    if (!result.IsSuccessStatusCode)
                    {
                        throw new Exception($"Failed to get gateway URL: {result.ReasonPhrase}");
                    }
                    JObject payload = JObject.Parse(await result.Content.ReadAsStringAsync());
                    _gatewayUrl = payload["url"]?.ToString() ?? _initalGatewayUrl;
                } else _gatewayUrl = gatewayUrl;

                if (_initalGatewayUrl == String.Empty) _initalGatewayUrl = _gatewayUrl;

                await _ws.ConnectAsync(
                    new Uri(_gatewayUrl),
                    _cts.Token);

                Connected?.Invoke(this, EventArgs.Empty);

                _receiveTask = ReceiveMessagesAsync();
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, ex);
                throw;
            }
        }

        public async Task Resume()
        {
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
        }

        public async Task DisconnectAsync(bool reconnecting = true)
        {
            _parentClient.ready = false;

            _cts.Cancel();

            if (_ws.State == WebSocketState.Open)
                await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnecting", CancellationToken.None);

            _heartbeatTask = null;
            _receiveTask = null;
            _cts = new CancellationTokenSource();

            if (!reconnecting) { dead = true; }
        }

        private async Task ReceiveMessagesAsync()
        {
            byte[]? buffer = new byte[8192];
            StringBuilder messageBuffer = new ();

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
                        string? message = messageBuffer.ToString();
                        messageBuffer.Clear();

                        await HandleMessageAsync(message);
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
                        } else await ConnectAsync(_initalGatewayUrl);

                        Disconnected?.Invoke(this, EventArgs.Empty);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, ex);
                Disconnected?.Invoke(this, EventArgs.Empty);
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
                    await SendHeartbeatAsync();
                    break;

                case 10: // Hello
                    _heartbeatInterval = payload["d"]?["heartbeat_interval"]?.Value<int>() ?? 41250;
                    _heartbeatTask = HeartbeatLoopAsync();
                    await SendIdentifyAsync();
                    break;

                case 11: // Heartbeat ACK
                    ping = (float)(DateTime.UtcNow.Ticks - _lastPingTicks) / 20000f;
                    _parentClient.ping = ping;
                    _heartbeatAcked = true;
                    break;

                case 7: // Reconnect
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
                        await ConnectAsync(_initalGatewayUrl);
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
            }
        }

        private async Task SendHeartbeatAsync()
        {
            var payload = new
            {
                op = 1,
                d = _sequence == -1 ? (int?)null : _sequence
            };

            await SendJsonAsync(payload);
        }

        private async Task SendIdentifyAsync()
        {
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
            string? json = JsonConvert.SerializeObject(data);
            byte[]? bytes = Encoding.UTF8.GetBytes(json);

            await _ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, _cts.Token);
        }
    }
}

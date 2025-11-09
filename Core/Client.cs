using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Net.WebSockets;
using System.Reflection;
using YellowMacaroni.Discord.API;
using YellowMacaroni.Discord.Cache;
using YellowMacaroni.Discord.Core;
using YellowMacaroni.Discord.Extentions;
using YellowMacaroni.Discord.Sharding;
using YellowMacaroni.Discord.Websocket;
using YellowMacaroni.Discord.Websocket.Events;

namespace YellowMacaroni.Discord.Core
{
    public class Client
    {
        private readonly WebsocketClient _ws;

        /// <summary>
        /// The ping of the websocket connection (one direction) in milliseconds to the nearest 10,000th of a millisecond.
        /// </summary>
        public float ping = -1;
        /// <summary>
        /// Whether the client is ready to use. This is true when the client has received the READY event from Discord.
        /// </summary>
        public bool ready;
        /// <summary>
        /// When the last ping (heartbeat) was sent out.
        /// </summary>
        public double lastPingTicks = -1d; // -1 if no ping, otherwise last ping timestamp as ticks  

        public DateTimeOffset startupTime;

        private readonly string token;
        private readonly Intents intents;

        public readonly Shard? shard;

        public Ready? readyData = null;

        public event EventHandler? ClientReady;
        public event EventHandler? Disconnected;
        public event EventHandler? Connecting;
        public event EventHandler? Connected;
        public event EventHandler<Exception>? Error;
        public event EventHandler<ClientDebug>? Debug;


        // EVENTS
        #region ClientEvents
        public event EventHandler<JObject>? Hello;
        public event EventHandler<Ready>? Ready;
        public event EventHandler? Resumed;
        public event EventHandler? Reconnect;
        public event EventHandler<bool>? InvalidSession;
        public event EventHandler<ApplicationCommandPermissions>? ApplicationCommandPermissionsUpdate;

        // Automod
        public event EventHandler<AutomodRule>? AutomoderationRuleCreate;
        public event EventHandler<AutomodRule>? AutomoderationRuleUpdate;
        public event EventHandler<AutomodRule>? AutomoderationRuleDelete;

        // Channels
        public event EventHandler<Channel>? ChannelCreate;
        public event EventHandler<Channel>? ChannelUpdate;
        public event EventHandler<Channel>? ChannelDelete;
        public event EventHandler<Channel>? ThreadCreate;
        public event EventHandler<Channel>? ThreadUpdate;
        public event EventHandler<Channel>? ThreadDelete;
        public event EventHandler<ThreadListSync>? ThreadListSync;
        public event EventHandler<ThreadMember>? ThreadMemberUpdate;
        public event EventHandler<ThreadMembersUpdate>? ThreadMembersUpdate;
        public event EventHandler<ChannelPinsUpdate>? ChannelPinsUpdate;

        // Entitlements
        public event EventHandler<Entitlement>? EntitlementCreate;
        public event EventHandler<Entitlement>? EntitlementUpdate;
        public event EventHandler<Entitlement>? EntitlementDelete;

        // Guilds
        public event EventHandler<Guild>? GuildCreate;
        public event EventHandler<Guild>? GuildUpdate;
        public event EventHandler<Guild>? GuildDelete;
        public event EventHandler<AuditLogEntry>? GuildAuditLogEntryCreate;
        public event EventHandler<GuildBan>? GuildBanAdd;
        public event EventHandler<GuildBan>? GuildBanRemove;
        public event EventHandler<GuildEmojiUpdate>? GuildEmojisUpdate;
        public event EventHandler<GuildStickerUpdate>? GuildStickersUpdate;
        public event EventHandler<JustGuildID>? GuildIntegrationsUpdate;
        public event EventHandler<Member>? GuildMemberAdd;
        public event EventHandler<Member>? GuildMemberRemove;
        public event EventHandler<Member>? GuildMemberUpdate;
        public event EventHandler<GuildMembersChunk>? GuildMembersChunk;
        public event EventHandler<GuildRole>? GuildRoleCreate;
        public event EventHandler<GuildRole>? GuildRoleUpdate;
        public event EventHandler<GuildRoleDelete>? GuildRoleDelete;
        public event EventHandler<ScheduledEvent>? GuildScheduledEventCreate;
        public event EventHandler<ScheduledEvent>? GuildScheduledEventUpdate;
        public event EventHandler<ScheduledEvent>? GuildScheduledEventDelete;
        public event EventHandler<EventUser>? GuildScheduledEventUserAdd;
        public event EventHandler<EventUser>? GuildScheduledEventUserRemove;
        public event EventHandler<SoundboardSound>? GuildSoundboardSoundCreate;
        public event EventHandler<SoundboardSound>? GuildSoundboardSoundUpdate;
        public event EventHandler<SoundboardSoundDelete>? GuildSoundboardSoundDelete;
        public event EventHandler<SoundboardSounds>? GuildSoundboardSoundsUpdate;
        public event EventHandler<SoundboardSounds>? SoundboardSounds;

        // Integrations
        public event EventHandler<Integration>? IntegrationCreate;
        public event EventHandler<Integration>? IntegrationUpdate;
        public event EventHandler<IntegrationDelete>? IntegrationDelete;

        // Invites
        public event EventHandler<InviteCreate>? InviteCreate;
        public event EventHandler<InviteDelete>? InviteDelete;

        // Messages
        public event EventHandler<Message>? MessageCreate;
        public event EventHandler<Message>? MessageUpdate;
        public event EventHandler<MessageDelete>? MessageDelete;
        public event EventHandler<MessageDeleteBulk>? MessageDeleteBulk;
        public event EventHandler<MessageReactionUpdate>? MessageReactionAdd;
        public event EventHandler<MessageReactionUpdate>? MessageReactionRemove;
        public event EventHandler<MessageReactionUpdate>? MessageReactionRemoveAll;
        public event EventHandler<MessageReactionUpdate>? MessageReactionRemoveEmoji;

        // Presence
        public event EventHandler<PresenceUpdate>? PresenceUpdate;
        public event EventHandler<TypingStart>? TypingStart;
        public event EventHandler<User>? UserUpdate;

        // Voice
        public event EventHandler<VoiceEffectSend>? VoiceChannelEffectSend;
        public event EventHandler<VoiceState>? VoiceStateUpdate;
        public event EventHandler<VoiceServerUpdate>? VoiceServerUpdate;

        // Webhooks
        public event EventHandler<WebhookUpdate>? WebhooksUpdate;

        // Interactions
        public event EventHandler<Interaction>? InteractionCreate;

        // Stage Instances
        public event EventHandler<StageInstance>? StageInstanceCreate;
        public event EventHandler<StageInstance>? StageInstanceUpdate;
        public event EventHandler<StageInstance>? StageInstanceDelete;

        // Subscriptions
        public event EventHandler<Subscription>? SubscriptionCreate;
        public event EventHandler<Subscription>? SubscriptionUpdate;
        public event EventHandler<Subscription>? SubscriptionDelete;

        // Polls
        public event EventHandler<PollVote>? MessagePollVoteAdd;
        public event EventHandler<PollVote>? MessagePollVoteRemove;
        #endregion

        // EVENT FUNCTIONS
        internal Dictionary<string, Action<Client, JObject>> eventHandlers = new() {
            {
                "GUILDCREATE",
                (client, data) =>
                {
                    Guild? guild = data.ToObject<Guild>();

                    if (guild is null) return;

                    DiscordCache.Guilds.UpdateOrInsert(guild.id, guild);
                }
            },
            {
                "GUILDUPDATE",
                (client ,data) =>
                {
                    Guild? guild = data.ToObject<Guild>();

                    if (guild is null) return;

                    DiscordCache.Guilds.UpdateOrInsert(guild.id, guild);
                }
            },
            {
                "GUILDDELETE",
                (client, data) =>
                {
                    string? guildId = data["id"]?.Value<string>();
                    if (guildId is null) return;
                    DiscordCache.Guilds.Remove(guildId);
                }
            },
            {
                "GUILDMEMBERSCHUNK",
                (client, data) =>
                {
                    GuildMembersChunk? chunk = data.ToObject<GuildMembersChunk>();
                    if (chunk is null) return;
                    Guild? guild = DiscordCache.Guilds.Get(chunk.guild_id).WaitFor();
                    if (guild is null) return;
                    foreach (var member in chunk.members) guild.members.Insert(member.user?.id ?? "", member);
                }
            },
            {
                "GUILDROLECREATE",
                (client, data) =>
                {
                    GuildRole? role = data.ToObject<GuildRole>();
                    if (role is null) return;
                    Guild? guild = DiscordCache.Guilds.Get(role.guild_id).WaitFor();
                    if (guild is null) return;
                    guild.roles.Insert(role.role.id, role.role);
                }
            },
            {
                "GUILDROLEUPDATE",
                (client, data) =>
                {
                    GuildRole? role = data.ToObject<GuildRole>();
                    if (role is null) return;
                    Guild? guild = DiscordCache.Guilds.Get(role.guild_id).WaitFor();
                    if (guild is null) return;
                    guild.roles.UpdateOrInsert(role.role.id, role.role);
                }
            },
            {
                "GUILDROLEDELETE",
                (client, data) =>
                {
                    GuildRoleDelete? roleDelete = data.ToObject<GuildRoleDelete>();
                    if (roleDelete is null) return;
                    Guild? guild = DiscordCache.Guilds.Get(roleDelete.guild_id).WaitFor();
                    if (guild is null) return;
                    guild.roles.Remove(roleDelete.role_id);
                }
            },
            {
                "TYPINGSTART",
                (client, data) =>
                {
                    TypingStart? typing = data.ToObject<TypingStart>();
                    if (typing is null) return;
                    DiscordCache.Channels.Get(typing.channel_id).WaitFor();
                }
            }
        };


        /// <summary>
        /// Create a new client using the provided token, for sharding, use the ShardingManager.
        /// </summary>
        /// <param name="token">The token to use for the client.</param>
        /// <param name="intents">The intents for the client to use.</param>
        public Client(string token, Intents intents, Shard? shard = null)
        {
            this.token = token;
            this.intents = intents;
            this.shard = shard;
            _ws = new WebsocketClient(this, token, intents);
            APIHandler.AddDefaultHeader("Authorization", $"Bot {token}");

            _ws.Dispatch += (websocket, data) =>
            {
                if (data.TryGetValue("t", out var eventName) && data.TryGetValue("d", out var payload) && eventName.Type != JTokenType.Null)
                {
                    string eventType = (eventName.Value<string>() ?? "").Replace("_", "");

                    var eventHandler = eventHandlers.GetValueOrDefault(eventType);
                    if (eventHandler is not null) try { eventHandler(this, JObject.FromObject(payload)); } catch { }

                    var events = this.GetType().GetEvents(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var eventInfo in events)
                    {
                        if (eventInfo.Name.Equals(eventType, StringComparison.OrdinalIgnoreCase))
                        {
                            Type? handlerType = eventInfo.EventHandlerType;
                            Type? dataType = handlerType?.GetGenericArguments().FirstOrDefault();

                            if (dataType is not null)
                            {
                                try
                                {
                                    object convertedData = payload.ToObject(dataType) ?? new { };

                                    var fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                                    var eventField = fields.FirstOrDefault(f => f.FieldType == eventInfo.EventHandlerType);
                                    var eventDelegate = eventField?.GetValue(this);

                                    if (eventDelegate is not null)
                                    {
                                        object[] parameters = { this, convertedData };

                                        eventDelegate?.GetType()?.GetMethod("Invoke")?.Invoke(eventDelegate, parameters);
                                    }
                                }
                                catch { }
                            }

                            break;
                        }
                    }
                }
            };

            _ws.Ready += (ws, _) =>
            {
                ClientReady?.Invoke(this, EventArgs.Empty);
            };

            _ws.Disconnected += (ws, _) =>
            {
                Disconnected?.Invoke(this, EventArgs.Empty);
            };

            _ws.Connecting += (ws, _) =>
            {
                Connecting?.Invoke(this, EventArgs.Empty);
            };

            _ws.Connected += (ws, _) =>
            {
                Connected?.Invoke(this, EventArgs.Empty);
            };

            _ws.Debug += (ws, debug) =>
            {                 
                Debug?.Invoke(this, debug);
            };

            _ws.ErrorOccurred += (ws, ex) =>
            {
                Error?.Invoke(this, ex);
            };
        }

        /// <summary>
        /// Connect to the Discord websocket.
        /// </summary>
        public void Connect() => _ws.ConnectAsync().Wait();

        /// <summary>
        /// Disconnect from the Discord websocket (will stop most functionality of the client).
        /// </summary>
        public void Disconnect() => _ws.DisconnectAsync().Wait();

        /// <summary>
        /// Assign a procedure to a specific dispatch event.
        /// </summary>
        /// <param name="eventName">The event name to attach to.</param>
        /// <param name="action">The procedure to run when the specified event is recieved.</param>
        public void On(string eventName, Action<Client, JObject> action)
        {
            _ws.Dispatch += (websocket, payload) =>
            {
                string thisEventName = payload["t"]?.ToString() ?? string.Empty;
                if (thisEventName == eventName)
                {
                    if (payload["d"] is JObject data)
                    {
                        action(this, data);
                    }
                }
            };
        }

        /// <summary>
        /// Connects the client to the Discord websocket and doesn't return until the client is disconnected (fully).
        /// </summary>
        public void Start()
        {
            Connect();
        }

        public async Task SendCommand(object data)
        {
            await _ws.SendJsonAsync(data);
        }
    }
}

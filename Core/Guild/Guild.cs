using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.Cache;

namespace YellowMacaroni.Discord.Core
{
    public class Guild(string guildId)
    {
        /// <summary>
        /// The guild ID.
        /// </summary>
        public string id = guildId;

        /// <summary>
        /// The name of the guild (from 2-100 characters, excluding leading and trailing whitespace).
        /// </summary>
        public string? name;

        /// <summary>
        /// The icon hash of the guild.
        /// </summary>
        public string? icon;

        /// <summary>
        /// The icon hash of the guild (templates).
        /// </summary>
        public string? icon_hash;

        /// <summary>
        /// The splash hash of the guild.
        /// </summary>
        public string? splash;

        /// <summary>
        /// The discovert splash hash of the guild.
        /// </summary>
        public string? discovery_splash;

        /// <summary>
        /// Whether the current user is the owner of the guild.
        /// </summary>
        public bool? owner;

        /// <summary>
        /// The user ID of the owner of the guild.
        /// </summary>
        public string? owner_id;

        /// <summary>
        /// The total permissions for the current user in the guild (excluding overwrites and implicit permissions).
        /// </summary>
        public string? permissions;

        /// <summary>
        /// The region of the guild (deprecated).
        /// </summary>
        public string? region;

        /// <summary>
        /// The ID of the AFK channel.
        /// </summary>
        public string? afk_channel_id;

        /// <summary>
        /// The AFK timeout in seconds.
        /// </summary>
        public int? afk_timeout;

        /// <summary>
        /// Whether the guild has the widget enabled.
        /// </summary>
        public bool? widget_enabled;

        /// <summary>
        /// The channel which the widget will point to.
        /// </summary>
        public string? widget_channel_id;

        /// <summary>
        /// The verification level of the guild as a <see cref="GuildVerificationLevel"/> enum.
        /// </summary>
        public GuildVerificationLevel? verification_level;

        /// <summary>
        /// The default message notification level of the guild as a <see cref="GuildDefaultMessageNotificationLevel"/> enum.
        /// </summary>
        public GuildDefaultMessageNotificationLevel? default_message_notifications;

        /// <summary>
        /// The explicit content filter level of the guild as a <see cref="GuildContentFilterLevel"/> enum.
        /// </summary>
        public GuildContentFilterLevel? explicit_content_filter;

        /// <summary>
        /// The channels in the guild.
        /// </summary>
        public List<Channel>? channels;

        [JsonProperty(nameof(roles))]
        private readonly List<Role>? _initialRoles;

        private Collection<Role>? _roles = null;

        /// <summary>
        /// The roles in the guild.
        /// </summary>
        [JsonIgnore]
        public Collection<Role> roles
        {
            get
            {
                _roles ??= new($"https://discord.com/api/v10/guilds/{id}/roles/{{key}}", cache: (_initialRoles ?? []).ToDictionary(r => r.id));
                return _roles;
            }
        }

        /// <summary>
        /// The emojis in the guild.
        /// </summary>
        public List<Emoji>? emojis;

        /// <summary>
        /// The guild features (array of <see cref="string"/>s).
        /// </summary>
        public string[]? features;

        /// <summary>
        /// The required MFA level for the guild as a <see cref="GuildMFALevel"/> enum.
        /// </summary>
        public GuildMFALevel? mfa_level;

        /// <summary>
        /// The application ID of the guild if it is a bot-created guild.
        /// </summary>
        public string? application_id;

        /// <summary>
        /// The ID of the channel where guild notices such as welcome messages and boost events are posted.
        /// </summary>
        public string? system_channel_id;

        /// <summary>
        /// 
        /// </summary>
        public GuildSystemChannelFlags? system_channel_flags;

        /// <summary>
        /// The ID of the channel where community guilds can display rules and/or guidelines.
        /// </summary>
        public string? rules_channel_id;

        /// <summary>
        /// The maximum number of presences for the guild (always null unless a very large guild).
        /// </summary>
        public int? max_presences;

        /// <summary>
        /// The maximum number of members for the guild.
        /// </summary>
        public int? max_members;

        /// <summary>
        /// The vanity URL code for the guild.
        /// </summary>
        public string? vanity_url_code;

        /// <summary>
        /// The description of the guild.
        /// </summary>
        public string? description;

        /// <summary>
        /// The banner hash of the guild.
        /// </summary>
        public string? banner;

        /// <summary>
        /// The premium tier of the guild as a <see cref="GuildPremiumTier"/> enum.
        /// </summary>
        public GuildPremiumTier? premium_tier;

        /// <summary>
        /// The number of boosts the guild has.
        /// </summary>
        public int? premium_subscription_count;

        /// <summary>
        /// The preferred locale of the guild (defaulted to 'en-US').
        /// </summary>
        public string? preferred_locale;

        /// <summary>
        /// The ID of the channel where admins and moderators of guilds will receive notices from Discord.
        /// </summary>
        public string? public_updates_channel_id;

        /// <summary>
        /// The maximum number of video channel users for the guild.
        /// </summary>
        public int? max_video_channel_users;

        /// <summary>
        /// The maximum number of video channel users for the guild (stage channels).
        /// </summary>
        public int? max_stage_video_channel_users;

        /// <summary>
        /// The approximate number of members in the guild when 'with_counts' is true.
        /// </summary>
        public int? approximate_member_count;

        /// <summary>
        /// The approximate number of online members in the guild when 'with_counts' is true.
        /// </summary>
        public int? approximate_presence_count;

        /// <summary>
        /// The welcome screen of a Community guild, shown to new members, returned in an invite's guild object.
        /// </summary>
        public WelcomeScreen? welcome_screen;

        /// <summary>
        /// The guilds NSFW level as a <see cref="GuildNSFWLevel"/> enum.
        /// </summary>
        public GuildNSFWLevel? nsfw_level;

        /// <summary>
        /// The stickers in the guild.
        /// </summary>
        public List<Sticker>? stickers;

        /// <summary>
        /// The guild's premium progress bar enabled.
        /// </summary>
        public bool? premium_progress_bar_enabled;

        /// <summary>
        /// The ID of the channel where admins and moderators of guilds will receive safety alerts from Discord.
        /// </summary>
        public string? safety_alerts_channel_id;

        /// <summary>
        /// The incidents data for this guild.
        /// </summary>
        public IncidentsData? incidents_data;

        /// <summary>
        /// When this guild was joined at.
        /// </summary>
        public string? joined_at;

        /// <summary>
        /// <see cref="true"/> if this is considered a large guild.
        /// </summary>
        public bool? large;

        /// <summary>
        /// <see cref="true"/> if this guild is unavilable due to an outage.
        /// </summary>
        public bool? unavailable;

        /// <summary>
        /// Total number of members in the guild.
        /// </summary>
        public double? member_count;

        /// <summary>
        /// States of members currently in voice channels.
        /// </summary>
        public List<VoiceState>? voice_states;

        private Collection<Member>? _members = null;

        /// <summary>
        /// The members in the guild.
        /// </summary>
        public Collection<Member> members {
            get
            {
                if (_members is not null) return _members;
                _members = new($"https://discord.com/api/v10/guilds/{id}/members/{{key}}");
                return _members;
            }
        }

        /// <summary>
        /// All active threads in the guild that the current user has access to.
        /// </summary>
        public List<Channel>? threads;

        /// <summary>
        /// Presences of the members in the guild, will only include non-offline members if the size is greater than the large threshold.
        /// </summary>
        public List<PresenceUpdate>? presences;

        /// <summary>
        /// Stage instances in the guild.
        /// </summary>
        public List<StageInstance>? stage_instances;

        /// <summary>
        /// Scheduled events in the guild.
        /// </summary>
        public List<ScheduledEvent>? guild_scheduled_events;

        /// <summary>
        /// Soundboard sounds in the guild.
        /// </summary>
        public List<SoundboardSound>? soundboard_sounds;
    }

    public class WelcomeScreen
    {
        /// <summary>
        /// The server description shown in the welcome screen.
        /// </summary>
        public string? description;

        /// <summary>
        /// The channels shown in the welcome screen (max 5).
        /// </summary>
        public WelcomeChannel[]? welcome_channels;
    }

    public class WelcomeChannel
    {
        /// <summary>
        /// The ID of the channel.
        /// </summary>
        public string? channel_id;

        /// <summary>
        /// The description shown for the channel.
        /// </summary>
        public string? description;

        /// <summary>
        /// The ID of the custom emoji (if it exists).
        /// </summary>
        public string? emoji_id;

        /// <summary>
        /// The emoji name if a custom emoji, the unicode character is standard, or null if no emoji is set.
        /// </summary>
        public string? emoji_name;
    }

    public class StageInstance (string instanceId)
    {
        public string id = instanceId;
        public string? guild_id;
        public string? channel_id;
        public string? topic;
        public EventPrivacyLevel? privacy_level;
        public bool? discoverable_disabled;
        public string? guild_scheduled_event_id;
    }
}

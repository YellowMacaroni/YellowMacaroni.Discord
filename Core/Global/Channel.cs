using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.API;
using YellowMacaroni.Discord.Cache;
using YellowMacaroni.Discord.Extentions;

namespace YellowMacaroni.Discord.Core
{
    public class Channel(string channelId)
    {
        /// <summary>
        /// The ID of the channel.
        /// </summary>
        public string id = channelId;

        /// <summary>
        /// The type of the channel as a <see cref="ChannelType"/> enum.
        /// </summary>
        public ChannelType? type;

        /// <summary>
        /// The ID of the parent guild.
        /// </summary>
        public string? guild_id;

        /// <summary>
        /// The guild this channel belongs to. Null if the channel is not in a guild.
        /// </summary>
        public Guild? guild => guild_id is not null ? DiscordCache.Guilds.Get(guild_id).WaitFor() : null;

        /// <summary>
        /// The position of the channel in the guild, channels with the same position are sorted by channel id.
        /// </summary>
        public int? position;

        /// <summary>
        /// Explicit permission overwrites for members and roles in the channel.
        /// </summary>
        public List<PermissionOverwrite>? permission_overwrites;

        /// <summary>
        /// The name of the channel.
        /// </summary>
        public string? name;

        /// <summary>
        /// The topic (description) of the channel.
        /// </summary>
        public string? topic;

        /// <summary>
        /// Whether the channel is marked as NSFW.
        /// </summary>
        public bool? nsfw;

        /// <summary>
        /// The ID of the last message in the channel.
        /// </summary>
        public string? last_message_id;

        /// <summary>
        /// The bitrate of the channel (in bits), only applicable for voice channels.
        /// </summary>
        public int? bitrate;

        /// <summary>
        /// The maximum number of users allowed in the voice channel.
        /// </summary>
        public int? user_limit;

        /// <summary>
        /// The amount of seconds a user has to wait before sending another message (0-21600). Bots amd users with the MANAGE_MESSAGES or MANAGE_CHANNEL permissions are unaffected.
        /// </summary>
        public int? rate_limit_per_user;

        /// <summary>
        /// The recipients of the DM. Only applicable for DM channels.
        /// </summary>
        public List<User>? recipients;

        /// <summary>
        /// The icon of the channel, only applicable for group DM channels.
        /// </summary>
        public string? icon;

        /// <summary>
        /// The ID of the user that created the group DM channel.
        /// </summary>
        public string? owner_id;

        /// <summary>
        /// The owner of the user that created the group DM channel.
        /// </summary>
        public User? owner => owner_id is not null ? DiscordCache.Users.Get(owner_id).WaitFor() : null;

        /// <summary>
        /// The ID of the application that created the group DM channel if it is bot created.
        /// </summary>
        public string? application_id;

        /// <summary>
        /// Whether the group DM channel is managed by an application via the gdm.join OAuth2 scope.
        /// </summary>
        public bool? managed;

        /// <summary>
        /// The parent category ID of the channel, parent channel if a thread.
        /// </summary>
        public string? parent_id;

        /// <summary>
        /// The parent category of the channel, parent channel if a thread.
        /// </summary>
        public Channel? parent => parent_id is not null ? DiscordCache.Channels.Get(parent_id).WaitFor() : null;

        /// <summary>
        /// The ISO8601 timestamp of the last pin in the channel.
        /// </summary>
        public string? last_ping_timestamp;

        /// <summary>
        /// The voice region ID for the voice channel, automatic when set to null.
        /// </summary>
        public string? rtc_region;

        /// <summary>
        /// The video quality mode of the channel as a <see cref="VideoQualityMode"/> enum.
        /// </summary>
        public VideoQualityMode? video_quality_mode;

        /// <summary>
        /// The number of messages (not including the initial or deleted messages) in a thread.
        /// </summary>
        public int? message_count;

        /// <summary>
        /// An approximate count of users in a thread, stops counting at 50.
        /// </summary>
        public int? member_count;

        /// <summary>
        /// Thread-specific fields not needed by any other channels.
        /// </summary>
        public ThreadMetadata? thread_metadata;

        /// <summary>
        /// Thread member object for the current user, if they have joined the thread (only certain API endpoints).
        /// </summary>
        public Member? member;

        /// <summary>
        /// The default auto archive duration of a thread, in minutes. Can be set to: 60, 1440, 4320, 10080.
        /// </summary>
        public int? default_auto_archive_duration;

        /// <summary>
        /// Computed permissions for the invoking user in the channel, including overwrites, roles, and permissions from parent channels.
        /// </summary>
        public string? permissions;

        /// <summary>
        /// Converts the <see cref="permissions"/> bit set to a <see cref="Flags{Permissions}"/> object.
        /// </summary>
        public Flags<Permissions> Permissions => new(permissions ?? "0");

        /// <summary>
        /// The channel flags as a <see cref="ChannelFlags"/> enum.
        /// </summary>
        public ChannelFlags? flags;

        /// <summary>
        /// Number of messages every sent in a thread, similar to <see cref="message_count"/> but will not decrement the number when a message is deleted.
        /// </summary>
        public int? total_message_sent;

        /// <summary>
        /// The set of tags that can be used in a <see cref="ChannelType.GuildForum"/> channel.
        /// </summary>
        public List<ChannelTag>? available_tags;

        /// <summary>
        /// The IDs of the set of tags that have been applied to a thread in a <see cref="ChannelType.GuildForum"/> channel.
        /// </summary>
        public List<string>? applied_tags;

        /// <summary>
        /// The emoji that is used as a default reaction for a <see cref="ChannelType.GuildForum"/> channel.
        /// </summary>
        public DefaultReaction? default_reaction_emoji;

        /// <summary>
        /// The initial <see cref="rate_limit_per_user"/> to set on newly created threads in a <see cref="ChannelType.GuildForum"/> channel.
        /// </summary>
        public int? default_thread_rate_limit_per_user;

        /// <summary>
        /// The default sort order to be used for threads in a <see cref="ChannelType.GuildForum"/> channel.
        /// </summary>
        public ForumSortOrderType? default_sort_order;

        /// <summary>
        /// The default layout type to be used for threads in a <see cref="ChannelType.GuildForum"/> channel.
        /// </summary>
        public ForumLayoutType? default_forum_layout;

        public async Task<(Message?, DiscordError?)> Send(object content)
        {
            
            HttpResponseMessage result = await APIHandler.POST($"/channels/{id}/messages", new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json"));

            return APIHandler.DeserializeResponse<Message>(result);
        }
        public async Task<(Message?, DiscordError?)> Send(string content)
        {
            return await Send(new { content });
        }
    }

    public class ChannelTag (string tagId)
    {
        /// <summary>
        /// The ID of the tag.
        /// </summary>
        public string id = tagId;

        /// <summary>
        /// The name of the tag.
        /// </summary>
        public string? name;

        /// <summary>
        /// Whether this tag can only be added to or removed from threads by a <see cref="Member"/> with the MANAGE_THREADS permission.
        /// </summary>
        public bool? moderated;

        /// <summary>
        /// The ID of the guild's custom emoji.
        /// </summary>
        public string? emoji_id;

        /// <summary>
        /// The unicode character of the emoji.
        /// </summary>
        public string? emoji_name;
    }

    public class DefaultReaction
    {
        /// <summary>
        /// The ID of a guild's custom emoji.
        /// </summary>
        public string? emoji_id;

        /// <summary>
        /// The unicode character for the emoji.
        /// </summary>
        public string? emoji_name;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.Core;

namespace YellowMacaroni.Discord.Websocket.Events
{
    /// <summary>
    /// Add: All fields<br/>
    /// Remove: All fields but message_author_id and burst_colors.<br/>
    /// Remove All: channel_id, message_id, guild_id only<br/>
    /// Remove Emoji: channel_id, guild_id, message_id, emoji only
    /// </summary>
    public class MessageReactionUpdate
    {
        public string? user_id;
        public string? message_id;
        public string? channel_id;
        public string? guild_id;
        public Member? member;
        public Emoji? emoji;
        public string? message_author_id;
        public bool? burst;
        public List<string>? burst_colors;
        public ReactionType? type;
    }

    public enum ReactionType
    {
        Normal = 0,
        Burst = 1
    }
}

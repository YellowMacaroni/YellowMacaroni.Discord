using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.Core;

namespace YellowMacaroni.Discord.Websocket.Events
{
    public class InviteCreate
    {
        public string? channel_id;
        public string? code;
        public string? created_at;
        public string? guild_id;
        public User? inviter;
        public int? max_age;
        public int? max_uses;
        public InviteTargetType? target_type;
        public User? target_user;
        public Application? target_application;
        public bool? temporary;
        public int? uses;
    }

    public enum InviteTargetType
    {
        Stream = 1,
        EmbeddedApplication = 2
    }
}

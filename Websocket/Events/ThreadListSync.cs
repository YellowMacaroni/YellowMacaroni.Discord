using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.Core;

namespace YellowMacaroni.Discord.Websocket.Events
{
    public class ThreadListSync
    {
        public string? guild_id;
        public List<string>? channel_ids;
        public List<Channel>? threads;
        public List<ThreadMember>? members;
    }

    public class ThreadMember
    {
        public string? id;
        public string? user_id;
        public string? guild_id;
        public string? join_timestamp;
        public int? flags;
        public Member? member;
    }

    public class ThreadMembersUpdate
    {
        public string? id;
        public string? guild_id;
        public int? member_count;
        public List<ThreadMember>? added_members;
        public List<string>? removed_member_ids;
    }
}

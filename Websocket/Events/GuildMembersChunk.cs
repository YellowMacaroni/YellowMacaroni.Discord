using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.Core;

namespace YellowMacaroni.Discord.Websocket.Events
{
    public class GuildMembersChunk
    {
        public string guild_id = "";
        public List<Member> members = [];
        public int chunk_index;
        public int chunk_count;
        public List<string>? not_found;
        public List<PresenceUpdate>? presences;
        public string? nonce;
    }
}

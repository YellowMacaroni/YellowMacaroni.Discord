using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Websocket.Events
{
    public class GuildRoleDelete
    {
        public string? guild_id;
        public string? role_id;
    }
}

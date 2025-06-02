using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Websocket.Events
{
    public class InviteDelete
    {
        public string? channel_id;
        public string? guild_id;
        public string? code;
    }
}

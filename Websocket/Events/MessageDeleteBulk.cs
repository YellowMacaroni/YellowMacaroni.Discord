using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Websocket.Events
{
    public class MessageDeleteBulk
    {
        public List<string>? ids;
        public string? channel_id;
        public string? guild_id;
    }
}

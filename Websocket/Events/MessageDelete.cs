using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Websocket.Events
{
    public class MessageDelete
    {
        public string? id;
        public string? channel;
        public string? guild_id;
    }
}

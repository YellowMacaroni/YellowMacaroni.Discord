using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Websocket.Events
{
    public class VoiceServerUpdate
    {
        public string? token;
        public string? guild_id;
        public string? endpoint;
    }
}

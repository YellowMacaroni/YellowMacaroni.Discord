using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Websocket.Events
{
    public class ChannelPinsUpdate
    {
        public string? guild_id;
        public string? channel_id;
        public string? last_pin_timestamp;
    }
}

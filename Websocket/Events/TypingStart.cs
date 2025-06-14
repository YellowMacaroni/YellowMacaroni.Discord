using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.Core;

namespace YellowMacaroni.Discord.Websocket.Events
{
    public class TypingStart
    {
        public string channel_id = "";
        public string? guild_id;
        public string user_id = "";
        /// <summary>
        /// Unix timestamp (seconds)
        /// </summary>
        public double timestamp = 0;
        public Member? member;
    }
}

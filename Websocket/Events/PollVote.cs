using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Websocket.Events
{
    public class PollVote
    {
        public string? user_id;
        public string? channel_id;
        public string? message_id;
        public string? guild_id;
        public string? answer_id;
    }
}

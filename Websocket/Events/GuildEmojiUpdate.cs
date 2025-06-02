using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.Core;

namespace YellowMacaroni.Discord.Websocket.Events
{
    public class GuildEmojiUpdate
    {
        public string? guild_id;
        public List<Emoji>? emojis;
    }
}

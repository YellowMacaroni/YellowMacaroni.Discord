using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.Core;

namespace YellowMacaroni.Discord.Websocket.Events
{
    public class GuildStickerUpdate
    {
        public string? guild_id;
        public List<Sticker>? stickers;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.Core;

namespace YellowMacaroni.Discord.Websocket.Events
{
    public class VoiceEffectSend
    {
        public string? channel_id;
        public string? guild_id;
        public string? user_id;
        public Emoji? emoji;
        public AnimationType? animation_type;
        public int? animation_id;
        public string? sound_id;
        public double? sound_volume;
    }

    public enum AnimationType
    {
        Premium = 0,
        Basic = 1
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class VoiceState
    {
        public string? guild_id;
        public string? channel_id;
        public string? user_id;
        public Member? member;
        public string? session_id;
        public bool? deaf;
        public bool? mute;
        public bool? self_deaf;
        public bool? self_mute;
        public bool? self_stream;
        public bool? self_video;
        public bool? suppress;
        public string? request_to_speak_timestamp;
    }
}

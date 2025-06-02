using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class AutomodRule
    {
        public string? id;
        public string? guild_id;
        public string? name;
        public string? creator_id;
        public EventType? event_type;
        public TriggerType? trigger_type;
        public TriggerMetadata? trigger_metadata;
        public List<AutomodAction>? actions;
        public bool? enabled;
        public List<string>? exempt_roles;
        public List<string>? exempt_channels;
    }

    public enum EventType
    {
        MessageSend = 1,
        MemberUpdate = 2
    }

    public enum TriggerType
    {
        Keyword = 1,
        Spam = 3,
        KeywordPreset = 4,
        MentionSpam = 5,
        MemberProfile = 6
    }

    public class TriggerMetadata
    {
        public List<string>? keyword_filter;
        public List<string>? regex_patterns;
        public List<KeywordPresetType>? presets;
        public List<string>? allow_list;
        public int? mention_total_limit;
        public bool? mention_raid_protection_enabled;
    }

    public enum KeywordPresetType
    {
        Profanity = 1,
        SexualContent = 2,
        Slurs = 3
    }

    public class AutomodAction
    {
        public ActionType? type;
        public ActionMetadata? metadata;
    }

    public enum ActionType
    {
        BlockMessage = 1,
        SendAlertMessage = 2,
        Timeout = 3,
        BlockMemberInteraction = 4
    }

    public class ActionMetadata
    {
        public string? channel_id;
        public int? duration_seconds;
        public string? custom_message;
    }
}

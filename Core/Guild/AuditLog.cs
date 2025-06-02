using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class AuditLogEntry (string entryId)
    {
        public string id = entryId;
        public string? target_id;
        public List<AuditLogChange>? changes;
        public string? user_id;
        public AuditActionType? action_type;
        public AuditEntryInfo? options;
        public string? reason;
    }

    public class AuditLogChange
    {
        public string? key;
        public dynamic? old_value;
        public dynamic? new_value;
    }

    public class AuditEntryInfo
    {
        public string? application_id;
        public string? auto_moderation_rule_name;
        public string? auto_moderation_rule_trigger_type;
        public string? channel_id;
        public string? count;
        public string? delete_member_days;
        public string? id;
        public string? members_removed;
        public string? message_id;
        public string? role_name;
        /// <summary>
        /// Role "0" or member "1"
        /// </summary>
        public string? type;
        public string? integration_type;
    }

    public enum AuditActionType
    {
        GuildUpdate = 1,
        ChannelCreate = 10,
        ChannelUpdate = 11,
        ChannelDelete = 12,
        ChannelOverwriteCreate = 13,
        ChannelOverwriteUpdate = 14,
        ChannelOverwriteDelete = 15,
        MemberKick = 20,
        MemberPrune = 21,
        MemberBanAdd = 22,
        MemberBanRemove = 23,
        MemberUpdate = 24,
        MemberRoleUpdate = 25,
        MemberMove = 26,
        MemberDisconnect = 27,
        BotAdd = 28,
        RoleCreate = 30,
        RoleUpdate = 31,
        RoleDelete = 32,
        InviteCreate = 40,
        InviteUpdate = 41,
        InviteDelete = 42,
        WebhookCreate = 50,
        WebhookUpdate = 51,
        WebhookDelete = 52,
        EmojiCreate = 60,
        EmojiUpdate = 61,
        EmojiDelete = 62,
        MessageDelete = 72,
        MessageBulkDelete = 73,
        MessagePin = 74,
        MessageUnpin = 75,
        IntegrationCreate = 80,
        IntegrationUpdate = 81,
        IntegrationDelete = 82,
        StageInstanceCreate = 83,
        StageInstanceUpdate = 84,
        StageInstanceDelete = 85,
        StickerCreate = 90,
        StickerUpdate = 91,
        StickerDelete = 92,
        GuildScheduledEventCreate = 100,
        GuildScheduledEventUpdate = 101,
        GuildScheduledEventDelete = 102,
        ThreadCreate = 110,
        ThreadUpdate = 111,
        ThreadDelete = 112,
        ApplicationCommandPermissionsUpdate = 121,
        SoundboardSoundCreate = 130,
        SoundboardSoundUpdate = 131,
        SoundboardSoundDelete = 132,
        AutoModerationRuleCreate = 140,
        AutoModerationRuleUpdate = 141,
        AutoModerationRuleDelete = 142,
        AutoModerationBlockMessage = 143,
        AutoModerationFlagToChannel = 144,
        AutoModerationUserCommunicationDisabled = 145,
        CreatorMonetizationRequestCreate = 150,
        CreatorMonetizationTermsAccepted = 151,
        OnboardingPromptCreate = 163,
        OnboardingPromptUpdate = 164,
        OnboardingPromptDelete = 165,
        OnboardingCreate = 166,
        OnboardingUpdate = 167,
        HomeSettingsCreate = 190,
        HomeSettingsUpdate = 191,
    }
}

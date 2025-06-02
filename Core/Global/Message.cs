using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.Cache;
using YellowMacaroni.Discord.Extentions;

namespace YellowMacaroni.Discord.Core
{
    public class Message
    {
        public string id = "";
        public string? channel_id;
        public Channel? channel => channel_id is not null ? DiscordCache.Channels.Get(channel_id).WaitFor() : null;
        public User? author;
        public string? content;
        public string? timestamp;
        public string? edited_timestamp;
        public bool? tts;
        public bool? mention_everyone;
        public List<User>? mentions;
        public List<string>? mention_roles;
        public List<ChannelMention>? mention_channels;
        public List<MessageAttachment>? attachments;
        public List<Embed>? embeds;
        public List<Reaction>? reactions;
        public string? nonce;
        public bool? pinned;
        public string? webhook_id;
        public MessageType? type;
        public MessageActivity? activity;
        public Application? application;
        public string? application_id;
        public MessageFlags flags;
        public MessageReference? message_reference;
        public List<MessageSnaphot>? message_snaphots;
        public Message? referenced_message;
        public MessageInteractionMetadata? interaction_metadata;
        public Interaction? interaction;
        public Channel? thread;
        public List<Component>? components;
        public List<StickerItem>? sticker_items;
        public List<Sticker>? stickers;
        public int? position;
        public RoleSubscriptionData? role_subscription_data;
        public ResolvedData? resovled;
        public Poll? poll;
        public MessageCall? call;

        public MessageBuilder ToBuilder ()
        {
            return new MessageBuilder(this);
        }
    }

    public class MessageBuilder: Message
    {
        public MessageBuilder() { }

        public MessageBuilder(Message message)
        {
            foreach (var prop in message.GetType().GetProperties())
            {
                var value = prop.GetValue(message);
                if (value is not null)
                {
                    this.GetType().GetProperty(prop.Name)?.SetValue(this, value);
                }
            }
        }

        public MessageBuilder (params Embed[] embed)
        {
            embeds = [ ..embed ];
        }

        public MessageBuilder (string message)
        {
            content = message;
        }

        public MessageBuilder SetContent(string content)
        {
            this.content = content;
            return this;
        }

        public MessageBuilder AddAttachment(params MessageAttachment[] attachment)
        {
            attachments ??= [];
            attachments.AddRange(attachment);
            return this;
        }

        public MessageBuilder AddEmbed(params Embed[] embed)
        {
            embeds ??= [];
            embeds.AddRange(embed);
            return this;
        }

        public MessageBuilder SetPoll (Poll poll)
        {
            this.poll = poll;
            return this;
        }
    }

    public class MessageSnaphot
    {
        public Message? message;
    }

    public class ChannelMention (string channelId)
    {
        public string id = channelId;
        public string? guild_id;
        public ChannelType? type;
        public string? name;
    }

    public class MessageActivity
    {
        public MessageActivityType type;
        public string? party_id;
    }

    public enum MessageActivityType
    {
        Join = 1,
        Spectate = 2,
        Listen = 3,
        JoinRequest = 5
    }

    public class MessageCall
    {
        public List<string>? participants;
        public string? ended_timestamp;
    }

    public class MessageAttachment (string attachmentId)
    {
        public string id = attachmentId;
        public string? filename;
        public string? title;
        public string? description;
        public string? content_type;
        public int? size;
        public string? url;
        public string? proxy_url;
        public int? height;
        public int? width;
        public bool? ephemeral;
        public float? duration_secs;
        public string? waveform;
        public AttachmentFlags? flags;
    }

    public class MessageReference
    {
        public MessageReferenceType? type;
        public string? message_id;
        public string? channel_id;
        public Channel? channel => channel_id is not null ? DiscordCache.Channels.Get(channel_id).WaitFor() : null;
        public string? guild_id;
        public Guild? guild => guild_id is not null ? DiscordCache.Guilds.Get(guild_id).WaitFor() : null;
        public bool? fail_if_not_exists;
    }

    public class MessageInteractionMetadata
    {
        public string? id;
        public InteractionType? type;
        public User? user;
        public Dictionary<string, ApplicationIntegrationType>? authorizing_integration_owners;
        public string? original_response_message_id;
        public User? target_user;
        public string? target_message_id;
    }

    public enum MessageReferenceType
    {
        Default = 0,
        Forward = 1
    }

    public enum AttachmentFlags
    {
        IsRemix = 1 << 2
    }

    public enum MessageType
    {
        Default = 0,
        RecipientAdd = 1,
        RecipientRemove = 2,
        Call = 3,
        CahnnelNameChange = 4,
        ChannelIconChange = 5,
        ChannelPinnedMessage = 6,
        UserJoin = 7,
        GuildBoost = 8,
        GuildBoostTier1 = 9,
        GuildBoostTier2 = 10,
        GuildBoostTier3 = 11,
        ChannelFollowAdd = 12,
        GuildDiscoveryDisqualified = 14,
        GuildDiscoveryRequalified = 15,
        GuildDiscoveryGracePeriodInitialWarning = 16,
        GuildDiscoveryGracePeriodFinalWarning = 17,
        ThreadCreated = 18,
        Reply = 19,
        ChatInputCommand = 20,
        ThreadStarterMessage = 21,
        GuildInviteReminder = 22,
        ContextMenuCommand = 23,
        AutoModerationAction = 24,
        RoleSubscriptionPurchase = 25,
        InteractionPremiumUpsell = 26,
        StageStart = 27,
        StageEnd = 28,
        StageSpeaker = 29,
        StageTopic = 31,
        GuildApplicationPremiumSubscription = 32,
        GuildIncidentAlertModeEnabled = 36,
        GuildIncidentAlertModeDisabled = 37,
        GuildIncidentReportRaid = 38,
        GuildIncidentReportFalseAlarm = 39,
        PurchaseNotification = 44,
        PollResult = 46
    }

    [Flags]
    public enum MessageFlags
    {
        Crossposted = 1 << 0,
        IsCrosspost = 1 << 1,
        SuppressEmbeds = 1 << 2,
        SourceMessageDeleted = 1 << 3,
        Urgent = 1 << 4,
        HasThread = 1 << 5,
        Ephemeral = 1 << 6,
        Loading = 1 << 7,
        FailedToMentionSomeRolesInThread = 1 << 8,
        SuppressNotification = 1 << 9,
        IsVoiceMessage = 1 << 13,
        HasSnapshot = 1 << 14,
        IsComponentsV2 = 1 << 15,
    }
}

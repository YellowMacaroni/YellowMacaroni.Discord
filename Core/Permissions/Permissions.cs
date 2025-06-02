using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    /// <summary>
    /// Discord's guild permissions.<br />
    /// Allowed channel abbreviations: T = Text, V = Voice, S = Stage.
    /// </summary>
    public enum Permissions
    {
        /// <summary>
        /// Allows creation of instant invites.<br/>T, V, S.
        /// </summary>
        CreateInstantInvite = 0,

        /// <summary>
        /// Allows kicking members.
        /// </summary>
        KickMembers = 1,

        /// <summary>
        /// Allows banning members.
        /// </summary>
        BanMembers = 2,

        /// <summary>
        /// Allows all permissions and bypasses channel permission overwrites.
        /// </summary>
        Administrator = 3,

        /// <summary>
        /// Allows management and editing of channels.<br/>T, V, S.
        /// </summary>
        ManageChannels = 4,

        /// <summary>
        /// Allows management and editing of the guild.
        /// </summary>
        ManageGuild = 5,

        /// <summary>
        /// Allows for adding new reactions to messages. This permission does not apply to reacting with an existing reaction on a message.<br/>
        /// T, V, S.
        /// </summary>
        AddReactions = 6,

        /// <summary>
        /// Allows for viewing of audit logs.
        /// </summary>
        ViewAuditLog = 7,

        /// <summary>
        /// Allows for using priority speaker in a voice channel.<br/>V.
        /// </summary>
        PrioritySpeaker = 8,

        /// <summary>
        /// Allows the user to go live.<br/>V, S.
        /// </summary>
        Stream = 9,

        /// <summary>
        /// Allows guild members to view a channel, which includes reading messages in text channels and joining voice channels.<br/>T, V, S.
        /// </summary>
        ViewChannel = 10,

        /// <summary>
        /// Allows for sending messages in a channel.<br/>T, V, S.
        /// </summary>
        SendMessages = 11,

        /// <summary>
        /// Allows for sending of '/tts' messages in a channel.<br/>T, V, S.
        /// </summary>
        SendTTSMessages = 12,

        /// <summary>
        /// Allows for deletion of other member's messages.<br/>T, V, S.
        /// </summary>
        ManageMessages = 13,

        /// <summary>
        /// Links sent by users with this permission will be auto-embedded.<br/>T, V, S.
        /// </summary>
        EmbedLinks = 14,

        /// <summary>
        /// Allows for uploading images, videos, and files.<br/>T, V, S.
        /// </summary>
        AttachFiles = 15,

        /// <summary>
        /// Allows for reading messages in a channel.<br/>T, V, S.
        /// </summary>
        ReadMessageHistory = 16,

        /// <summary>
        /// Allows for mentioning @everyone, @here, and all roles in a channel.<br/>T, V, S.
        /// </summary>
        MentionEveryone = 17,

        /// <summary>
        /// Allows the usage of custom emojis from other servers.<br/>T, V, S.
        /// </summary>
        UseExternalEmojis = 18,

        /// <summary>
        /// Allows for viewing guild insights.
        /// </summary>
        ViewGuildInsights = 19,

        /// <summary>
        /// Allows for connecting to voice channels.<br/>V, S.
        /// </summary>
        Connect = 20,

        /// <summary>
        /// Allows for speaking in voice channels.<br/>V.
        /// </summary>
        Speak = 21,

        /// <summary>
        /// Allows for muting members in voice channels.<br/>V, S.
        /// </summary>
        MuteMembers = 22,

        /// <summary>
        /// Allows for deafening members in voice channels.<br/>V, S.
        /// </summary>
        DeafenMembers = 23,

        /// <summary>
        /// Allows for moving members between voice channels.<br/>V, S.
        /// </summary>
        MoveMembers = 24,

        /// <summary>
        /// Allows for using voice-activity-detection in a voice channel.<br/>V.
        /// </summary>
        UseVAD = 25,

        /// <summary>
        /// Allows for modification of own nickname.
        /// </summary>
        ChangeNickname = 26,

        /// <summary>
        /// Allows for modification of other members nicknames.
        /// </summary>
        ManageNicknames = 27,

        /// <summary>
        /// Allows for management and editing of roles.<br/>T, V, S.
        /// </summary>
        ManageRoles = 28,

        /// <summary>
        /// Allows for management and editing of webhooks.<br/>T, V, S.
        /// </summary>
        ManageWebhooks = 29,
        
        /// <summary>
        /// Allows for editing and deleting emojis, stickers, and soundboard sounds created by all users.
        /// </summary>
        ManageExpressions = 30,

        /// <summary>
        /// Allows for using application commands in text channels.<br/>T, V, S.
        /// </summary>
        UseApplicationCommands = 31,

        /// <summary>
        /// Allows for requesting to speak in stage channels.<br/>S.
        /// </summary>
        RequestToSpeak = 32,

        /// <summary>
        /// Allows for editing and deleting scheduled events created by all users.<br/>V, S.
        /// </summary>
        ManageEvents = 33,

        /// <summary>
        /// Allows for deleting and archiving threads, and viewing all private threads.<br/>T.
        /// </summary>
        ManageThreads = 34,

        /// <summary>
        /// Allows for creating public and announcement threads.<br/>T.
        /// </summary>
        CreatePublicThreads = 35,

        /// <summary>
        /// Allows for creating private threads.<br/>T.
        /// </summary>
        CreatePrivateThreads = 36,

        /// <summary>
        /// Allows for using custom stickers from other servers.<br/>T, V, S.
        /// </summary>
        UseExternalStickers = 37,

        /// <summary>
        /// Allows for sending messages in threads.<br/>T.
        /// </summary>
        SendMessagesInThreads = 38,

        /// <summary>
        /// Allows for starting and managing activities in a voice channel.<br/>T, V.
        /// </summary>
        StartEmbeddedActivities = 39,

        /// <summary>
        /// Allows for timing out users to prevent them from sending or reacting to messages in chat and threads, and from speaking in voice and stage channels.
        /// </summary>
        ModerateMembers = 40,

        /// <summary>
        /// Allows for viewing role subscription insights.
        /// </summary>
        ViewCreatorMonetizationAnalytics = 41,

        /// <summary>
        /// Allows for using soundboard in a voice channel.<br/>V.
        /// </summary>
        UseSoundboard = 42,

        /// <summary>
        /// Allows for creating emojis, stickers, and soundboard sounds, and editing and deleteting those created by the current user.
        /// </summary>
        CreateExpressions = 43,

        /// <summary>
        /// Allows for creating scheduled events, and editing and deleting those created by the current user.
        /// </summary>
        CreateEvents = 44,

        /// <summary>
        /// Allows the usage of custom soundboard sounds from other servers.<br/>V.
        /// </summary>
        UseExternalSounds = 45,

        /// <summary>
        /// Allows sending voice messages.<br/>T, V, S.
        /// </summary>
        SendVoiceMessages = 46,

        /// <summary>
        /// Allows for sending polls.<br/>T, V, S.
        /// </summary>
        SendPolls = 49,

        /// <summary>
        /// Allows user-installed apps to send public responses. When disabled, users will still be allowed to use their apps but the responses will be ephemeral. This only applies to apps not already installed on the server.<br/>T, V, S.
        /// </summary>
        UseExternalApps = 50
    }
}

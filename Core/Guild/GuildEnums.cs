using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public enum GuildDefaultMessageNotificationLevel
    {
        AllMessages = 0,
        MentionsOnly = 1
    }

    public enum GuildContentFilterLevel
    {
        Disabled = 0,
        MembersWithoutRoles = 1,
        AllMembers = 2
    }

    public enum GuildMFALevel
    {
        None = 0,
        Elevated = 1
    }

    public enum GuildVerificationLevel
    {
        None = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        VeryHigh = 4
    }

    public enum GuildNSFWLevel
    {
        Default = 0,
        Explicit = 1,
        Safe = 2,
        AgeRestricted = 3
    }

    public enum GuildPremiumTier
    {
        None = 0,
        Tier1 = 1,
        Tier2 = 2,
        Tier3 = 3
    }

    [Flags]
    public enum GuildSystemChannelFlags
    {
        SuppressJoinNotifications = 1 << 0,
        SuppressPremiumSubscriptions = 1 << 1,
        SuppressGuildReminderNotifications = 1 << 2,
        SuppressJoinNotificationReplies = 1 << 3,
        SuppressRoleSubscriptionPurchaseNotifications = 1 << 4,
        SuppressRoleSubscriptionPurchaseNotificationReplies = 1 << 5,
    }
}

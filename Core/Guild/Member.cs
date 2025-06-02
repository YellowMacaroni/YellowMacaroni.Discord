using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class Member
    {
        /// <summary>
        /// The user this guild member represents.
        /// </summary>
        public User? user;

        /// <summary>
        /// The user's guild nickname.
        /// </summary>
        public string? nick;

        /// <summary>
        /// The user's guild avatar hash.
        /// </summary>
        public string? avatar;

        /// <summary>
        /// The user's guild banner hash.
        /// </summary>
        public string? banner;

        /// <summary>
        /// The user's roles.
        /// </summary>
        public List<string>? roles;

        /// <summary>
        /// ISO8601 timestamp of when the user joined.
        /// </summary>
        public string? joined_at;

        /// <summary>
        /// ISO8601 timestamp of when the user started boosting the guild.
        /// </summary>
        public string? premium_since;

        /// <summary>
        /// Whether the user has been deafened in voice channels.
        /// </summary>
        public bool? deaf;

        /// <summary>
        /// Whether the user has been muted in voice channels.
        /// </summary>
        public bool? mute;

        /// <summary>
        /// Guild member flags.
        /// </summary>
        public MemberFlags? flags;

        /// <summary>
        /// Whether the user has not yet passed the guild's membership screening requirements.
        /// </summary>
        public bool? pending;

        /// <summary>
        /// The total permissions of the member in the channel, including overwrites, interactions only.
        /// </summary>
        public string? permissions;

        /// <summary>
        /// Converts the <see cref="permissions"/> bit set to a <see cref="Flags{Permissions}"/> object.
        /// </summary>
        public Flags<Permissions> Permissions => new(permissions ?? "0");

        /// <summary>
        /// ISO8601 timestamp representing when the user's timeout wil expire. Will be null or a time in the past if not timed out.
        /// </summary>
        public string? communication_disabled_until;

        /// <summary>
        /// The data for the member's guild avatar decoration.
        /// </summary>
        public AvatarDecoration? avatar_decoration_data;

        /// <summary>
        /// The guild ID this member is in, used for gateway events.
        /// </summary>
        public string? guild_id;
    }

    public enum MemberFlags
    {
        /// <summary>
        /// The user has left and rejoined the guild.
        /// </summary>
        DidRejoin = 1 << 0,

        /// <summary>
        /// The user has completed onboarding.
        /// </summary>
        CompletedOnboarding = 1 << 1,

        /// <summary>
        /// The member is exempt from guild verification requirements.
        /// </summary>
        BypassesVerification = 1 << 2,

        /// <summary>
        /// The user has started onboarding.
        /// </summary>
        StartedOnboarding = 1 << 3,

        /// <summary>
        /// The member is a guest and can only access the voice channel they were invited to.
        /// </summary>
        IsGuest = 1 << 4,

        /// <summary>
        /// Member has started server guide new member actions.
        /// </summary>
        StartedHomeActions = 1 << 5,

        /// <summary>
        /// Member has completed server guide new member actions.
        /// </summary>
        CompletedHomeActions = 1 << 6,

        /// <summary>
        /// The member's username, display name, or nickname is blocked by automod.
        /// </summary>
        AutomodQuarantinedUsername = 1 << 7,

        /// <summary>
        /// The member has dismissed the DM settings upsell.
        /// </summary>
        DMSettingsUpselAcknowledged = 1 << 9
    }
}

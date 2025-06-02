using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class User (string userId)
    {
        /// <summary>
        /// The ID of the user.
        /// </summary>
        public string id = userId;

        /// <summary>
        /// The username of the user, unique.
        /// </summary>
        public string? username;

        /// <summary>
        /// The user's 4-digit Discord tag, no longer in use and will always be '0'.
        /// </summary>
        public string? discriminator;

        /// <summary>
        /// The user's display name. For bots, this is the application's name.
        /// </summary>
        public string? global_name;

        /// <summary>
        /// The user's avatar hash.
        /// </summary>
        public string? avatar;

        /// <summary>
        /// Whether the user belongs to an OAuth2 application.
        /// </summary>
        public bool? bot;

        /// <summary>
        /// Whether the user is an official Discord system user (part of the urgent message system).
        /// </summary>
        public bool? system;

        /// <summary>
        /// Whether the user has two factor enabled on their account.
        /// </summary>
        public bool? mfa_enabled;

        /// <summary>
        /// The user's banner hash.
        /// </summary>
        public string? banner;

        /// <summary>
        /// The user's banner color encoded as an integer representation of hexadecimal color code.
        /// </summary>
        public int? accent_color;

        /// <summary>
        /// The user's chosen language option.
        /// </summary>
        public string? locale;

        /// <summary>
        /// Whether the email on this account has been verified, requires the email OAuth scope.
        /// </summary>
        public bool? verified;

        /// <summary>
        /// The user's email, requires the email OAth scope.
        /// </summary>
        public string? email;

        /// <summary>
        /// The user's flags as a <see cref="UserFlags"/> enum.
        /// </summary>
        public UserFlags? flags;

        /// <summary>
        /// The user's premium type as a <see cref="PremiumType"/> enum.
        /// </summary>
        public PremiumType? premium_type;

        /// <summary>
        /// The user's public flags as a <see cref="UserFlags"/> enum.
        /// </summary>
        public UserFlags? public_flags;

        /// <summary>
        /// The user's avatar decoration data.
        /// </summary>
        public AvatarDecoration? avatar_decoration_data;

        /// <summary>
        /// The user's member in this current guild (only applicable in guilds).
        /// </summary>
        public Member? member;
    }

    public class AvatarDecoration
    {
        /// <summary>
        /// The avatar decoration hash.
        /// </summary>
        public string? asset;

        /// <summary>
        /// The ID of the avatar decoration's SKU
        /// </summary>
        public string? sku_id;
    }
}

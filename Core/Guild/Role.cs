using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class Role (string roleId)
    {
        /// <summary>
        /// The ID of the role.
        /// </summary>
        public string id = roleId;

        /// <summary>
        /// The name of the role.
        /// </summary>
        public string? name;

        /// <summary>
        /// The color of the role as an integer representation of the hexadeciminal color code.
        /// </summary>
        public int? color;

        /// <summary>
        /// The <see cref="color"/> as a <see cref="Core.Color"/> object.
        /// </summary>
        public Color Color
        {
            get => (Color)(color ?? 0);
            set => color = (int)value;
        }

        /// <summary>
        /// If the role is pinned in the user listing.
        /// </summary>
        public bool? hoist;

        /// <summary>
        /// The role icon hash.
        /// </summary>
        public string? icon;

        /// <summary>
        /// The roloe unicode emoji.
        /// </summary>
        public string? unicode_emoji;

        /// <summary>
        /// The position of the role. Roles with the same position are sorted by <see cref="id"/>.
        /// </summary>
        public int? position;

        /// <summary>
        /// The permissions bit set.
        /// </summary>
        public string? permissions;

        /// <summary>
        /// Converts the <see cref="permissions"/> bit set to a <see cref="Flags{Permissions}"/> object.
        /// </summary>
        public Flags<Permissions> Permissions => new(permissions ?? "0");

        /// <summary>
        /// Whether the role is managed by an integration.
        /// </summary>
        public bool? managed;

        /// <summary>
        /// Whether the role is mentionable (by users without the MANAGE_ROLES permission).
        /// </summary>
        public bool? mentionable;

        /// <summary>
        /// The tags this role has.
        /// </summary>
        public RoleTags? tags;

        /// <summary>
        /// The flags this role has as a <see cref="RoleFlags"/> enum.
        /// </summary>
        public RoleFlags? flags;
    }

    public class RoleTags
    {
        /// <summary>
        /// The ID of the bot this role belongs to.
        /// </summary>
        public string? bot_id;

        /// <summary>
        /// The ID of the integration this role belongs to.
        /// </summary>
        public string? integration_id;

        /// <summary>
        /// Whether this is the guild's booster role.
        /// </summary>
        public bool? premium_subsciber;

        /// <summary>
        /// The Id of this role's subscription SKU and listing.
        /// </summary>
        public string? subscription_listing_id;

        /// <summary>
        /// Whether this role is available for purchase.
        /// </summary>
        public bool? available_for_purchase;

        /// <summary>
        /// Whether this role is a guild's linked role.
        /// </summary>
        public bool? guild_connections;
    }
}

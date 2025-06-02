using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class PermissionOverwrite
    {
        /// <summary>
        /// The role or user ID for the overwrite.
        /// </summary>
        public string? id;

        /// <summary>
        /// The type of the overwrite, either <see cref="OverwriteType.Member"/> or <see cref="OverwriteType.Role"/>.
        /// </summary>
        public OverwriteType? type;

        /// <summary>
        /// The allow permission bit set for the overwrite.
        /// </summary>
        public string? allow;

        /// <summary>
        /// The deny permission bit set for the overwrite.
        /// </summary>
        public string? deny;

        /// <summary>
        /// Converts the <see cref="allow"/> permission bit set string to a <see cref="Flags{Permissions}"/> object.
        /// </summary>
        public Flags<Permissions> Allow => new(allow ?? "0");

        /// <summary>
        /// Converts the <see cref="deny"/> permission bit set string to a <see cref="Flags{Permissions}"/> object.
        /// </summary>
        public Flags<Permissions> Deny => new(deny ?? "0");
    }

    public enum OverwriteType
    {
        Role = 0,
        Member = 1
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class Emoji
    {
        /// <summary>
        /// The ID of the emoji.
        /// </summary>
        public string? id;
        
        /// <summary>
        /// The name of the emoji.
        /// </summary>
        public string? name;

        /// <summary>
        /// The roles that are allowed to use the emoji.
        /// </summary>
        public List<string>? roles;

        /// <summary>
        /// The user that created the emoji.
        /// </summary>
        public User? user;

        /// <summary>
        /// Whether the emoji must be wrapped in colons.
        /// </summary>
        public bool? require_colons;

        /// <summary>
        /// Whether the emoji is managed by an application.
        /// </summary>
        public bool? managed;

        /// <summary>
        /// Wherther the emoji is animated.
        /// </summary>
        public bool? animated;

        /// <summary>
        /// Whether the emoji can be used, may be false due to loss of server boosts.
        /// </summary>
        public bool? available;
    }
}

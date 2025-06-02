using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.Core;

namespace YellowMacaroni.Discord.Websocket.Events
{
    public class Ready
    {
        /// <summary>
        /// The API version being used.
        /// </summary>
        public int? v;

        /// <summary>
        /// Information about the current user (including email).
        /// </summary>
        public User? user;

        /// <summary>
        /// List of unavailable guild objects.
        /// </summary>
        public List<Guild>? guilds;

        /// <summary>
        /// Used for resuming connections.
        /// </summary>
        public string? session_id;

        /// <summary>
        /// Gateway URL for resuming connections.
        /// </summary>
        public string? resume_gateway_url;

        /// <summary>
        /// An array of two integers (shard_id, shard_count).
        /// </summary>
        public (int, int)? shard;

        /// <summary>
        /// The application which the bot is connected to.
        /// </summary>
        public Application? application;
    }
}

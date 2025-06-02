using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class ThreadMetadata
    {
        /// <summary>
        /// Whether the thread is archived.
        /// </summary>
        public bool? archived;

        /// <summary>
        /// The thread will stop showing in the channel list after <see cref="auto_archive_duration"/> minutes of inactivity, either: 60, 1440, 4320, or 10080.
        /// </summary>
        public int? auto_archive_duration;

        /// <summary>
        /// ISO8601 timestamp of when the thread's archive status was last changed.
        /// </summary>
        public string? archive_timestamp;

        /// <summary>
        /// Whether the thread is locked.
        /// </summary>
        public bool? locked;

        /// <summary>
        /// Whether non-moderators can add other non-moderators to a thread; only available on private threads.
        /// </summary>
        public bool? invitable;

        /// <summary>
        /// ISO8601 timestamp of when the thread was created.
        /// </summary>
        public string? create_timestamp;
    }
}

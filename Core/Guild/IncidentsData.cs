using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class IncidentsData
    {
        /// <summary>
        /// When invites get enabled again.
        /// </summary>
        public string? invites_disabled_until;

        /// <summary>
        /// When direct messages get enabled again.
        /// </summary>
        public string? dms_disabled_until;

        /// <summary>
        /// When the DM spam was detected.
        /// </summary>
        public string? dm_spam_detected_at;

        /// <summary>
        /// When the raid was detected.
        /// </summary>
        public string? raid_detected_at;
    }
}

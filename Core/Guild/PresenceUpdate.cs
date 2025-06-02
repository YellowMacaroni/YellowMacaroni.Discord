using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class PresenceUpdate
    {
        public User? user;
        public string? guild_id;
        public string? status;
        public List<Activity>? activities;
        public ClientStatus? client_status;
    }

    public class ClientStatus
    {
        public string? desktop;
        public string? mobile;
        public string? web;
    }

    public class Activity
    {
        public string? name;
        public ActivityType? type;
        public string? url;
        public int? created_at;
        public List<ActivityTimestamp>? timestamps;
        public string? application_id;
        public string? details;
        public string? state;
        public Emoji? emoji;
        public Party? party;
        public ActivityAssets? assets;
        public ActivitySecrets? secrets;
        public bool? instance;
        public ActivityFlags? flags;
        public List<ActivityButton>? buttons;
    }

    public enum ActivityType
    {
        Playing = 0,
        Streaming = 1,
        Listening = 2,
        Watching = 3,
        Custom = 4,
        Competing = 5
    }

    public class ActivityTimestamp
    {
        public int? start;
        public int? end;
    }

    public class Party
    {
        public string? id;
        public (int, int)? size;
    }

    public class ActivityAssets
    {
        public string? large_image;
        public string? large_text;
        public string? small_image;
        public string? small_text;
    }

    public class ActivitySecrets
    {
        public string? join;
        public string? spectate;
        public string? match;
    }

    public enum ActivityFlags
    {
        Instance = 1 << 0,
        Join = 1 << 1,
        Spectate = 1 << 2,
        JoinRequest = 1 << 3,
        Sync = 1 << 4,
        Play = 1 << 5,
        PartyPrivacyFriends = 1 << 6,
        PartyPrivacyVoice = 1 << 7,
        Embedded = 1 << 8,
    }

    public class ActivityButton
    {
        public string? label;
        public string? url;
    }
}

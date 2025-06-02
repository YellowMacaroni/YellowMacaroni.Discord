using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public enum ChannelType
    {
        GuildText = 0,
        DM = 1,
        GuildVoice = 2,
        GroupDM = 3,
        GuildCategory = 4,
        GuildAnnouncement = 5,
        AnnouncementThread = 10,
        PublicThread = 11,
        PrivateThread = 12,
        GuildStageVoice = 13,
        GuildDirectory = 14,
        GuildForum = 15,
        GuildStore = 16,
    }

    public enum VideoQualityMode
    {
        Auto = 1,
        Full = 2
    }

    [Flags]
    public enum ChannelFlags
    {
        Pinned = 1 << 1,
        RequiresTag = 1 << 4,
        HideMediaDownloadOptions = 1 << 15
    }

    public enum ForumSortOrderType
    {
        LatestActivity = 0,
        CreationDate = 1
    }

    public enum ForumLayoutType
    {
        NotSet = 0,
        ListView = 1,
        GalleryView = 2
    }
}

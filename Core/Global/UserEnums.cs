using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    [Flags]
    public enum UserFlags
    {
        Staff = 1 << 0,
        Partner = 1 << 1,
        HypeSquad = 1 << 2,
        BugHunterLevel1 = 1 << 3,
        HypesquadOnlineHouse1 = 1 << 6,
        HypesquadOnlineHouse2 = 1 << 7,
        HypesquadOnlineHouse3 = 1 << 8,
        PremiumEarlySupporter = 1 << 9,
        TeamPseudoUser = 1 << 10,
        BugHunterLevel2 = 1 << 14,
        VerifiedBot = 1 << 16,
        VerifiedBotDeveloper = 1 << 17,
        CertifiedModerator = 1 << 18,
        BotHTTPInteractions = 1 << 19,
        ActiveDeveloper = 1 << 22,
    }

    public enum PremiumType
    {
        None = 0,
        NitroClassic = 1,
        Nitro = 2,
        NitroBasic = 3,
    }
}

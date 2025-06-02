using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class GuildApplicationCommandPermissions
    {
        public string? id;
        public string? application_id;
        public List<ApplicationCommandPermissions>? permissions;
        public string? guild_id;
    }

    public class ApplicationCommandPermissions
    {
        public string? id;
        public bool? permission;
        public ApplicationCommandPermissionType? type;
    }

    public enum ApplicationCommandPermissionType
    {
        Role = 1,
        User = 2,
        Channel = 3
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class Integration (string integrationId)
    {
        public string id = integrationId;
        public string? name;
        public string? type;
        public bool? enabled;
        public bool? syncing;
        public string? role_id;
        public bool? enable_emoticons;
        public IntegrationExpireBehavior? expire_behavior;
        public int? expire_grace_period;
        public User? user;
        public IntegrationAccount? account;
        public string? synced_at;
        public int? subscriber_count;
        public bool? revoked;
        public Application? application;
        public List<string>? scopes;
        public string? guild_id;
    }

    public class IntegrationAccount
    {
        public string? id;
        public string? name;
    }

    public enum IntegrationExpireBehavior
    {
        RemoveRole = 0,
        Kick = 1
    }
}

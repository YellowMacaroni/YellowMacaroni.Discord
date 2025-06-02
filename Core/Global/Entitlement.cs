using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class Entitlement
    {
        public string? id;
        public string? sku_id;
        public string? application_id;
        public string? user_id;
        public EntitlementType? type;
        public bool? deleted;
        public string? starts_at;
        public string? ends_at;
        public string? guild_id;
        public bool? consumed;
    }

    public enum EntitlementType
    {
        Purchase = 1,
        PremiumSubscription = 2,
        DeveloperGift = 3,
        TestModePurchase = 4,
        FreePurchase = 5,
        UserGift = 6,
        PremiumPurchase = 7,
        ApplicationSubscription = 8,
    }

    public class RoleSubscriptionData
    {
        public string? role_subscription_listing_id;
        public string? tier_name;
        public int total_months_subscribed;
        public bool is_renweal;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class Subscription
    {
        /// <summary>
        /// The ID of the subscription.
        /// </summary>
        public string? id;

        /// <summary>
        /// The ID of the user that is subscribed.
        /// </summary>
        public string? user_id;

        /// <summary>
        /// List of SKUs subscribed to.
        /// </summary>
        public List<string>? sku_ids;

        /// <summary>
        /// List of entitlements granted for this subscription.
        /// </summary>
        public List<string>? entitlement_ids;

        /// <summary>
        /// List of SKUs that this user wil be subscribed to at renewal.
        /// </summary>
        public List<string>? renewal_sku_ids;

        /// <summary>
        /// ISO8601 timestamp for the start of the current subscription period.
        /// </summary>
        public string? current_period_start;

        /// <summary>
        /// ISO8601 timestamp for the end of the current subscription period.
        /// </summary>
        public string? current_period_end;

        /// <summary>
        /// Current status of the subscription.
        /// </summary>
        public SubscriptionStatus? status;

        /// <summary>
        /// ISO8601 timestamp for when the subscription was canceled.
        /// </summary>
        public string? canceled_at;

        /// <summary>
        /// ISO3166-1 alpha-2 country code of the payment source used to purchase the subscription. Missing unless queried with a private OAuth scope.
        /// </summary>
        public string? country;
    }

    public enum SubscriptionStatus
    {
        Active = 0,
        Ending = 1,
        Inactive = 2
    }
}

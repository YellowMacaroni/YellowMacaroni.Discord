using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class Application (string applicationId)
    {
        /// <summary>
        /// The ID of the application.
        /// </summary>
        public string id = applicationId;

        /// <summary>
        /// The name of the application.
        /// </summary>
        public string? name;

        /// <summary>
        /// The icon hash of the application.
        /// </summary>
        public string? icon;

        /// <summary>
        /// The description of the application.
        /// </summary>
        public string? description;

        /// <summary>
        /// A list of RPC origin URLs, if RPC is enabled.
        /// </summary>
        public List<string>? rpc_origins;

        /// <summary>
        /// When <see cref="false"/>, only the owner can add the app to guilds.
        /// </summary>
        public bool? bot_public;

        /// <summary>
        /// When <see cref="true"/>, the app's bot will only join upon completion of the full OAuth2 code grant flow.
        /// </summary>
        public bool? bot_require_code_grant;

        /// <summary>
        /// Partial user for the bot user associated with the app.
        /// </summary>
        public User? bot;

        /// <summary>
        /// URL of the app's Terms of Service.
        /// </summary>
        public string? terms_of_service_url;

        /// <summary>
        /// URL of the app's Privacy Policy.
        /// </summary>
        public string? privacy_policy_url;

        /// <summary>
        /// Partial user of the owner of the app.
        /// </summary>
        public User? owner;

        /// <summary>
        /// Hex encoded key for verification in interactions and the GameSDK's GetTicket.
        /// </summary>
        public string? verify_key;

        /// <summary>
        /// If the app belongs to a team, the app's team.
        /// </summary>
        public Team? team;

        /// <summary>
        /// The guild associated with the app, for example, a support server.
        /// </summary>
        public string? guild_id;

        /// <summary>
        /// A partial guild for he associated guild.
        /// </summary>
        public Guild? guild;

        /// <summary>
        /// If the app is a game sold on Discord, this field will be the ID of the SKU that is currently being sold.
        /// </summary>
        public string? primary_sku_id;

        /// <summary>
        /// If this app is a game sold on Discord, this field will be the URL slug that links to the store page.
        /// </summary>
        public string? slug;

        /// <summary>
        /// App's default rich presence invite cover image hash.
        /// </summary>
        public string? cover_image;

        /// <summary>
        /// App's public flags.
        /// </summary>
        public ApplicationFlags? flags;

        /// <summary>
        /// Approximate count of guilds the app has been added to.
        /// </summary>
        public int? approximate_guild_count;

        /// <summary>
        /// Approximate count of users that ahve installed the app.
        /// </summary>
        public int? approximate_user_install_count;

        /// <summary>
        /// Array of redirect URIs for the app.
        /// </summary>
        public List<string>? redirect_urls;

        /// <summary>
        /// The interactions endpoint URL for the app.
        /// </summary>
        public string? interactions_endpoint_url;

        /// <summary>
        /// The role connection verification URL for the app.
        /// </summary>
        public string? role_conenctions_verification_url;

        /// <summary>
        /// Event webhooks URL for the app to recieve webhook events.
        /// </summary>
        public string? event_webhooks_url;

        /// <summary>
        /// If webhook events are enabled for the app.
        /// </summary>
        public EventWebhookStatus? event_webhook_status;

        /// <summary>
        /// List of webhook event types the app subscribes to.
        /// </summary>
        public List<string>? event_webhooks_types;

        /// <summary>
        /// List of tags describing the context and functionality of the app (max 5).
        /// </summary>
        public List<string>? tags;

        /// <summary>
        /// Settings for the app's default in-app authorization link, if enabled.
        /// </summary>
        public InstallParams? install_params;

        /// <summary>
        /// Default scopes and permissions for each supported installation context.
        /// </summary>
        public Dictionary<string, Oauth2InstallParams>? integration_types_config;

        /// <summary>
        /// The default autorization URL for the app, if enabled.
        /// </summary>
        public string? custom_install_url;
    }

    public class Team (string teamId)
    {
        public string id = teamId;
        public string? icon;
        public string? name;
        public string? owner_user_id;
        public List<TeamMember>? members;
    }

    public struct TeamMember
    {
        public string team_id;
        public User user;
        public string role;
        public MembershipState state;
    }

    public class InstallParams
    {
        public List<string>? scopes;
        public string? permissions;
        public Flags<Permissions> Permissions => new(permissions ?? "0");
    }

    public class Oauth2InstallParams
    {
        public InstallParams? oauth2_install_params;
    }

    public enum MembershipState
    {
        Invited = 1,
        Accepted = 2
    }

    public enum ApplicationFlags
    {
        ApplicationAutoModerationRuleCreateBadge = 1 << 6,
        GatewayPresence = 1 << 12,
        GatewayPresenceLimited = 1 << 13,
        GatewayGuildMembers = 1 << 14,
        GatewayGuildMembersLimited = 1 << 15,
        VerificationPendingGuildLimit = 1 << 16,
        Embedded = 1 << 17,
        GatewayMessageContent = 1 << 18,
        GatewayMessageContentLimited = 1 << 19,
        ApplicationCommandBadge = 1 << 23,
    }

    public enum EventWebhookStatus
    {
        Disabled = 1,
        Enabled = 2,
        DisabledByDiscord = 3
    }

    public enum ApplicationIntegrationType
    {
        GuildInstall = 0,
        UserInstall = 1
    }
}

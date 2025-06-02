using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class Interaction
    {
        /// <summary>
        /// The ID of the interaction.
        /// </summary>
        public string? id;

        /// <summary>
        /// The Id of the application this interaction is for.
        /// </summary>
        public string? application_id;

        /// <summary>
        /// The type of interaction.
        /// </summary>
        public InteractionType? type;

        /// <summary>
        /// Interaction data payload
        /// </summary>
        public InteractionData? data;

        /// <summary>
        /// Guild that the interaction was sent from.
        /// </summary>
        public Guild? guild;

        /// <summary>
        /// Guild that the interaction was sent from.
        /// </summary>
        public string? guild_id;

        /// <summary>
        /// Channel that the interaction was sent from.
        /// </summary>
        public Channel? channel;

        /// <summary>
        /// Channel that the interaction was sent from.
        /// </summary>
        public string? channel_id;

        /// <summary>
        /// Guild member data for the invoking user, including permissions.
        /// </summary>
        public Member? member;

        /// <summary>
        /// User object for the invoking user, if invoked in a DM.
        /// </summary>
        public User? user;

        /// <summary>
        /// Continuation token for responding to the interaction.
        /// </summary>
        public string? token;

        /// <summary>
        /// Always 1.
        /// </summary>
        public int? version;

        /// <summary>
        /// For components, the message they were attached to.
        /// </summary>
        public Message? message;

        /// <summary>
        /// Bitwise set of permissions the app has in the source location of the interaction.
        /// </summary>
        public string? app_permissions;

        /// <summary>
        /// The <see cref="app_permissions"/> as a <see cref="Flags{Permissions}"/> object.
        /// </summary>
        public Flags<Permissions> AppPermissions { 
            get => (Flags<Permissions>)(app_permissions ?? "0");
            set => app_permissions = value.ToString(); 
        }

        /// <summary>
        /// The selected language for the invoking user.
        /// </summary>
        public string? locale;

        /// <summary>
        /// The guild's prefered locale, if invoked in a guild.
        /// </summary>
        public string? guild_locale;

        /// <summary>
        /// For monetized apps, any entitlements for the invoking user.
        /// </summary>
        public List<Entitlement>? entitlements;

        /// <summary>
        /// Mapping of installation contexts, that the interaction was authorized for to related user or guild IDs.
        /// </summary>
        public Dictionary<string, Oauth2InstallParams>? authorizing_integration_owners;

        /// <summary>
        /// Context where the interaction was triggered from.
        /// </summary>
        public InteractionContextType? context;

        /// <summary>
        /// Attachment size limit in bytes.
        /// </summary>
        public int? attachment_size_limit;
    }

    public enum InteractionType
    {
        Ping = 1,
        ApplicationCommand = 2,
        MessageComponent = 3,
        ApplicationCommandAutocomplete = 4,
        ModalSubmit = 5
    }

    public enum InteractionContextType
    {
        Guild = 9,
        BotDM = 1,
        PrivateChannel = 2
    }

    public class InteractionData
    {
        /// <summary>
        /// ID of the invoked command.
        /// </summary>
        public string? id;

        /// <summary>
        /// The name of the invoked command.
        /// </summary>
        public string? name;

        /// <summary>
        /// The type of command invoked.
        /// </summary>
        public InteractionCommandType? type;

        /// <summary>
        /// Converted users, roles, channels, and attachments.
        /// </summary>
        public ResolvedData? resolved;

        /// <summary>
        /// Params and values from the user.
        /// </summary>
        public List<ApplicationCommandInteractionDataOption>? options;

        /// <summary>
        /// The ID of the guild the command is registered to.
        /// </summary>
        public string? guild_id;

        /// <summary>
        /// ID of the user or message targeted by a user or message command.
        /// </summary>
        public string? target_id;
    }

    public enum InteractionCommandType
    {
        ChatInput = 1,
        User = 2,
        Message = 3,
        PrimaryEntryPoint = 4
    }

    public class ResolvedData
    {
        public Dictionary<string, User>? users;
        public Dictionary<string, Member>? members;
        public Dictionary<string, Role>? roles;
        public Dictionary<string, Channel>? channels;
        public Dictionary<string, Message>? messages;
        public Dictionary<string, MessageAttachment>? attachments;
    }

    public class ApplicationCommandInteractionDataOption
    {
        public string? name;
        public ApplicationCommandOptionType? type;
        public dynamic? value;
        public List<ApplicationCommandInteractionDataOption>? options;
        public bool? focused;
    }

    public enum ApplicationCommandOptionType
    {
        SubCommand = 1,
        SubCommandGroup = 2,
        String = 3,
        Integer = 4,
        Boolean = 5,
        User = 6,
        Channel = 7,
        Role = 8,
        Mentionable = 9,
        Number = 10,
        Attachment = 11
    }
}

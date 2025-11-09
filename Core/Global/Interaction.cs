using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public Dictionary<string, string>? authorizing_integration_owners;

        /// <summary>
        /// Context where the interaction was triggered from.
        /// </summary>
        public InteractionContextType? context;

        /// <summary>
        /// Attachment size limit in bytes.
        /// </summary>
        public int? attachment_size_limit;

        public async Task Callback(InteractionResponseType type, object? data)
        {
            await API.APIHandler.POST($"/interactions/{id}/{token}/callback", new StringContent(JsonConvert.SerializeObject(new InteractionResponse { type = type, data = data }), Encoding.UTF8, "application/json"));
        }

        public async Task Pong()
        {
            await Callback(InteractionResponseType.Pong, null);
        }
        
        public async Task Respond(MessageBuilder message)
        {
            await Callback(InteractionResponseType.ChannelMessageWithSource, message);
        }

        public async Task DeferResponse(bool ephemeral = false)
        {
            await Callback(InteractionResponseType.DeferredChannelMessageWithSource, ephemeral ? new { flags = 1 << 6 } : null);
        }

        public async Task DeferUpdate()
        {
            await Callback(InteractionResponseType.DeferredUpdateMessage, null);
        }

        public async Task AutocompleteResult(List<AutocompleteChoices> choices)
        {
            await Callback(InteractionResponseType.ApplicationCommandAutocompleteResult, new { choices });
        }

        public async Task ShowModal(ModalBuilder modal)
        {
            await Callback(InteractionResponseType.Modal, modal);
        }

        public async Task LaunchActivity()
        {
            await Callback(InteractionResponseType.LaunchActivity, null);
        }

        public async Task<(Message?, DiscordError?)> EditResponse(MessageBuilder message)
        {
            HttpResponseMessage result = await API.APIHandler.PATCH($"/webhooks/{application_id}/{token}/messages/@original", new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json"));
            return API.APIHandler.DeserializeResponse<Message>(result);
        }

        public async Task<(Message?, DiscordError?)> DeleteResponse()
        {
            HttpResponseMessage result = await API.APIHandler.DELETE($"/webhooks/{application_id}/{token}/messages/@original");
            return API.APIHandler.DeserializeResponse<Message>(result);
        }

        public async Task<(Message?, DiscordError?)> SendFollowup(MessageBuilder message)
        {
            HttpResponseMessage result = await API.APIHandler.POST($"/webhooks/{application_id}/{token}", new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json"));
            return API.APIHandler.DeserializeResponse<Message>(result);
        }

        public async Task<(Message?, DiscordError?)> EditFollowup(string messageId, object message)
        {
            HttpResponseMessage result = await API.APIHandler.PATCH($"/webhooks/{application_id}/{token}/messages/{messageId}", new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json"));
            return API.APIHandler.DeserializeResponse<Message>(result);
        }

        public async Task DeleteFollowup(string messageId)
        {
            await API.APIHandler.DELETE($"/webhooks/{application_id}/{token}/messages/{messageId}");
        }

        private T? GetComponentById<T>(string custom_id, ComponentType? type = null) where T : Component
        {
            foreach (Component component in data?.components ?? [])
            {
                Component? thisComponent = null;

                if (component.type == ComponentType.ActionRow)
                {
                    ActionRow? row = component as ActionRow;
                    thisComponent = row?.components?.FirstOrDefault();
                }
                else if (component.type == ComponentType.Label)
                {
                    Label? label = component as Label;
                    thisComponent = label?.component;
                }

                if (thisComponent?.custom_id == custom_id && (type is null || thisComponent.type == type)) return thisComponent as T;
            }

            return null;
        }


        public List<string>? GetStringSelectField(string custom_id)
        {
            StringSelect? select = GetComponentById<StringSelect>(custom_id, ComponentType.StringSelect);
            return select?.values;
        }

        public List<string>? GetAnySelectField(string custom_id)
        {
            Select? select = GetComponentById<Select>(custom_id);
            return select?.values;
        }

        public List<Role>? GetRoleField(string custom_id)
        {
            RoleSelect? select = GetComponentById<RoleSelect>(custom_id, ComponentType.RoleSelect);
            if (select?.values is null) return null;

            return [.. (data!.resolved?.roles?.Values.Where(r => select.values.Contains(r.id)) ?? [])];
        }

        public List<Channel>? GetChannelField(string custom_id)
        {
            ChannelSelect? select = GetComponentById<ChannelSelect>(custom_id, ComponentType.ChannelSelect);
            if (select?.values is null) return null;

            return [.. (data!.resolved?.channels?.Values.Where(ch => select.values.Contains(ch.id)) ?? [])];
        }

        public List<User>? GetUserField(string custom_id)
        {
            UserSelect? select = GetComponentById<UserSelect>(custom_id, ComponentType.UserSelect);
            if (select?.values is null) return null;

            return [.. (data!.resolved?.users?.Values.Where(u => select.values.Contains(u.id)) ?? [])];
        }

        public string? GetStringField(string custom_id)
        {
            TextInput? input = GetComponentById<TextInput>(custom_id, ComponentType.TextInput);
            return input?.value;
        }
    }

    public class InteractionResponse
    {
        public InteractionResponseType type;
        public object? data;
    }

    public class AutocompleteChoices
    {
        public string name = "";
        public string value = "";
        public Dictionary<string, string>? name_localizations;
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
        // APPLICATION_COMMAND

        /// <summary>
        /// [Application Command] ID of the invoked command.
        /// </summary>
        public string? id;

        /// <summary>
        /// [Application Command] The name of the invoked command.
        /// </summary>
        public string? name;

        /// <summary>
        /// [Application Command] The type of command invoked.
        /// </summary>
        public InteractionCommandType? type;


        /// <summary>
        /// [Application Command] Params and values from the user.
        /// </summary>
        public List<ApplicationCommandInteractionDataOption>? options;

        /// <summary>
        /// [Application Command] The ID of the guild the command is registered to.
        /// </summary>
        public string? guild_id;

        /// <summary>
        /// [Application Command] ID of the user or message targeted by a user or message command.
        /// </summary>
        public string? target_id;

        // MESSAGE_COMPONENT
        /// <summary>
        /// [Message Component] The type of component.
        /// </summary>
        public ComponentType? component_type;

        /// <summary>
        /// [Message Component] The values of the component, if it is a select menu.
        /// </summary>
        public List<string>? values;

        // MODAL_SUBMIT
        /// <summary>
        /// [Modal Submit] The components within the modal.
        /// </summary>
        [JsonProperty(ItemConverterType = typeof(ComponentConverter))]
        public List<Component>? components;


        // APPLICATION_COMMAND & MESSAGE_COMPONENT
        /// <summary>
        /// Converted users, roles, channels, and attachments.
        /// </summary>
        public ResolvedData? resolved;

        // MESSAGE_COMPONENT & MODAL_SUBMIT
        /// <summary>
        /// The custom_id of the component or modal.
        /// </summary>
        public string? custom_id;
    }

    public enum InteractionResponseType
    {
        Pong = 1,
        ChannelMessageWithSource = 4,
        DeferredChannelMessageWithSource = 5,
        DeferredUpdateMessage = 6,
        UpdateMessage = 7,
        ApplicationCommandAutocompleteResult = 8,
        Modal = 9,
        LaunchActivity = 12
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

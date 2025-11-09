using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowMacaroni.Discord.Extentions;

namespace YellowMacaroni.Discord.Core
{
    [JsonConverter(typeof(ComponentConverter))]
    public class Component (ComponentType type)
    {
        public ComponentType type = type;
        public ComponentType component_type = type;
        public int? id;
        public string custom_id = "";
    }

    public class ActionRow (): Component (ComponentType.ActionRow)
    {
        [JsonProperty(ItemConverterType = typeof(ComponentConverter))]
        public List<Component>? components;
    }

    public class ActionRowBuilder: ActionRow
    {
        public ActionRowBuilder() { }
        public ActionRowBuilder(params Component[] components)
        {
            this.components = [.. components];
        }
        public ActionRowBuilder(Component component)
        {
            components = [.. new List<Component> { component }];
        }
        public ActionRowBuilder(List<Component> components)
        {
            this.components = components;
        }

        public ActionRowBuilder SetComponents(List<Component> components)
        {
            this.components = components;
            return this;
        }
        public ActionRowBuilder AddComponent(params Component[] components)
        {
            this.components ??= [];
            this.components.AddRange(components);
            return this;
        }
    }

    public class Button(): Component(ComponentType.Button)
    {
        public ButtonStyle style;
        public string? label;
        public Emoji? emoji;
        public string? sku_id;
        public string? url;
        public bool? disabled;
    }

    public class ButtonBuilder: Button
    {
        /// <summary>
        /// Create a button with a custom ID
        /// </summary>
        /// <param name="customId">The custom ID for the button</param>
        public ButtonBuilder(string customId)
        {
            custom_id = customId;
        }
        
        /// <summary>
        /// Create a link button
        /// </summary>
        public ButtonBuilder()
        {
            style = ButtonStyle.Link;
        }

        public ButtonBuilder SetCustomId(string customId)
        {
            custom_id = customId;
            return this;
        }

        public ButtonBuilder SetLabel(string label)
        {
            this.label = label;
            return this;
        }

        public ButtonBuilder SetStyle(ButtonStyle style)
        {
            this.style = style;
            return this;
        }

        public ButtonBuilder SetEmoji(Emoji emoji)
        {
            this.emoji = emoji;
            return this;
        }

        public ButtonBuilder SetSkuId(string skuId)
        {
            this.sku_id = skuId;
            return this;
        }

        public ButtonBuilder SetUrl(string url)
        {
            this.url = url;
            return this;
        }

        public ButtonBuilder Disable()
        {
            disabled = true;
            return this;
        }

        public ButtonBuilder Enable()
        {
            disabled = false;
            return this;
        }
    }


    public enum ButtonStyle
    {
        Primary = 1,
        Secondary,
        Success,
        Danger,
        Link,
        Premium
    }

    public class StringSelect(): Component(ComponentType.StringSelect)
    {
        public List<StringSelectOption> options = [];
        public string? placeholder;
        public int? min_values;
        public int? max_values;
        public bool? disabled;
        public bool? required;
        public List<string> values = [];
    }

    public class StringSelectBuilder: StringSelect
    {
        public StringSelectBuilder(string customId)
        {
            custom_id = customId;
        }

        public StringSelectBuilder SetCustomId(string customId)
        {
            custom_id = customId;
            return this;
        }

        public StringSelectBuilder SetPlaceholder(string placeholder)
        {
            this.placeholder = placeholder;
            return this;
        }

        public StringSelectBuilder SetMinValues(int minValues)
        {
            min_values = minValues;
            return this;
        }

        public StringSelectBuilder SetMaxValues(int maxValues)
        {
            max_values = maxValues;
            return this;
        }

        public StringSelectBuilder Disable()
        {
            disabled = true;
            return this;
        }

        public StringSelectBuilder Enable()
        {
            disabled = false;
            return this;
        }

        public StringSelectBuilder SetRequired(bool required)
        {
            this.required = required;
            return this;
        }

        public StringSelectBuilder AddOption(params StringSelectOption[] options)
        {
            this.options.AddRange(options);
            return this;
        }

        public StringSelectBuilder SetOptions(List<StringSelectOption> options)
        {
            this.options = options;
            return this;
        }

        public StringSelectBuilder AddOption(string label, string value, string? description = null, Emoji? emoji = null, bool? @default = null)
        {
            options.Add(new StringSelectOption
            {
                label = label,
                value = value,
                description = description,
                emoji = emoji,
                @default = @default
            });
            return this;
        }
    }

    public class StringSelectOption
    {
        public string label = "";
        public string value = "";
        public string? description;
        public Emoji? emoji;
        [JsonProperty("default")]
        public bool? @default;
    }

    public class Select(ComponentType type): Component(type)
    {
        public string? placeholder;
        public List<SelectDefaultValue>? default_values;
        public int? min_values;
        public int? max_values;
        public bool? disabled;
        public bool? required;
        public List<string> values = [];
    }

    public class SelectBuilder<T>: Select where T: SelectBuilder<T>
    {
        public SelectBuilder(ComponentType type, string customId): base(type)
        {
            custom_id = customId;
        }

        public T SetCustomId(string customId)
        {
            custom_id = customId;
            return (T)this;
        }

        public T SetPlaceholder(string placeholder)
        {
            this.placeholder = placeholder;
            return (T)this;
        }

        public T SetMinValues(int minValues)
        {
            min_values = minValues;
            return (T)this;
        }

        public T SetMaxValues(int maxValues)
        {
            max_values = maxValues;
            return (T)this;
        }

        public T Disable()
        {
            disabled = true;
            return (T)this;
        }

        public T Enable()
        {
            disabled = false;
            return (T)this;
        }

        public T SetRequired(bool required)
        {
            this.required = required;
            return (T)this;
        }
    }

    public class UserSelect() : Select(ComponentType.UserSelect) { }
    public class RoleSelect() : Select(ComponentType.RoleSelect) { }
    public class MentionableSelect() : Select(ComponentType.MentionableSelect) { }
    public class ChannelSelect(): Select(ComponentType.ChannelSelect)
    {
        public List<ChannelType>? channel_types;
    }

    public class SelectDefaultValue
    {
        public string id = "";
        public string type = "user";
    }

    public class UserSelectBuilder(string customId): SelectBuilder<UserSelectBuilder>(ComponentType.UserSelect, customId)
    {
        public UserSelectBuilder AddDefaultValue(params string[] userId)
        {
            default_values ??= [];
            default_values.AddRange(userId.ForAll(
                (u) => new SelectDefaultValue { id = u, type = "user" }
            ));
            return this;
        }
        
        public UserSelectBuilder SetDefaultValues(List<string> userIds)
        {
            default_values = [.. userIds.ForAll(
                (u) => new SelectDefaultValue { id = u, type = "user" }
            )];
            return this;
        }
    }

    public class RoleSelectBuilder(string customId) : SelectBuilder<RoleSelectBuilder>(ComponentType.RoleSelect, customId)
    {
        public RoleSelectBuilder AddDefaultValue(params string[] roleId)
        {
            default_values ??= [];
            default_values.AddRange(roleId.ForAll(
                (r) => new SelectDefaultValue { id = r, type = "role" }
            ));
            return this;
        }
        public RoleSelectBuilder SetDefaultValues(List<string> roleIds)
        {
            default_values = [.. roleIds.ForAll(
                (r) => new SelectDefaultValue { id = r, type = "role" }
            )];
            return this;
        }
    }

    public class MentionableSelectBuilder(string customId): SelectBuilder<MentionableSelectBuilder>(ComponentType.MentionableSelect, customId)
    {
        public MentionableSelectBuilder AddDefaultUser(params string[] userId)
        {
            default_values ??= [];
            default_values.AddRange(userId.ForAll(
                (m) => new SelectDefaultValue { id = m, type = "user" }
            ));
            return this;
        }
        public MentionableSelectBuilder SetDefaultUsers(List<string> userIds)
        {
            default_values = [.. userIds.ForAll(
                (m) => new SelectDefaultValue { id = m, type = "user" }
            )];
            return this;
        }

        public MentionableSelectBuilder AddDefaultRole(params string[] roleId)
        {
            default_values ??= [];
            default_values.AddRange(roleId.ForAll(
                (m) => new SelectDefaultValue { id = m, type = "role" }
            ));
            return this;
        }
        public MentionableSelectBuilder SetDefaultRoles(List<string> roleIds)
        {
            default_values = [.. roleIds.ForAll(
                (m) => new SelectDefaultValue { id = m, type = "role" }
            )];
            return this;
        }
    }

    public class ChannelSelectBuilder(string customId): SelectBuilder<ChannelSelectBuilder>(ComponentType.ChannelSelect, customId)
    {
        public ChannelSelectBuilder AddDefaultValue(params string[] channelId)
        {
            default_values ??= [];
            default_values.AddRange(channelId.ForAll(
                (c) => new SelectDefaultValue { id = c, type = "channel" }
            ));
            return this;
        }
        public ChannelSelectBuilder SetDefaultValues(List<string> channelIds)
        {
            default_values = [.. channelIds.ForAll(
                (c) => new SelectDefaultValue { id = c, type = "channel" }
            )];
            return this;
        }
    }

    public class TextInput(): Component(ComponentType.TextInput)
    {
        public TextInputStyle style;
        public string label = "";
        public int? min_length;
        public int? max_length;
        public bool? required;
        public string? value;
        public string? placeholder;
    }

    public class TextInputBuilder: TextInput
    {
        public TextInputBuilder(string label, TextInputStyle style = TextInputStyle.Short)
        {
            this.label = label;
            this.style = style;
        }

        public TextInputBuilder SetLabel(string label)
        {
            this.label = label;
            return this;
        }

        public TextInputBuilder SetStyle(TextInputStyle style)
        {
            this.style = style;
            return this;
        }

        public TextInputBuilder SetMinLength(int minLength)
        {
            this.min_length = minLength;
            return this;
        }

        public TextInputBuilder SetMaxLength(int maxLength)
        {
            this.max_length = maxLength;
            return this;
        }

        public TextInputBuilder SetRequired(bool required)
        {
            this.required = required;
            return this;
        }

        public TextInputBuilder SetValue(string value)
        {
            this.value = value;
            return this;
        }

        public TextInputBuilder SetPlaceholder(string placeholder)
        {
            this.placeholder = placeholder;
            return this;
        }
    }

    public enum TextInputStyle
    {
        Short = 1,
        Paragraph = 2
    }

    public class Section(): Component(ComponentType.Section)
    {
        public List<TextDisplay> components = [];
        [JsonConverter(typeof(ComponentConverter))]
        public Component accessory = new(ComponentType.Button);
    }

    public class SectionBuilder: Section
    {
        public SectionBuilder() { }
        public SectionBuilder(params TextDisplay[] component)
        {
            components = [.. component];
        }
        public SectionBuilder(Component accessory)
        {
            this.accessory = accessory;
        }
        public SectionBuilder(List<TextDisplay> components, Component accessory)
        {
            this.components = [.. components];
            this.accessory = accessory;
        }

        public SectionBuilder SetComponents(List<TextDisplay> components)
        {
            this.components = [.. components];
            return this;
        }

        public SectionBuilder AddComponent(params TextDisplay[] component)
        {
            components.AddRange(component);
            return this;
        }

        public SectionBuilder SetAccessory(Component accessory)
        {
            this.accessory = accessory;
            return this;
        }
    }

    public class TextDisplay(): Component(ComponentType.TextDisplay)
    {
        public string content = "";
    }

    public class TextDisplayBuilder: TextDisplay
    {
        public TextDisplayBuilder(string content)
        {
            this.content = content;
        }
    }

    public class Thumbnail(): Component(ComponentType.Thumbnail)
    {
        public UnfurledMediaItem media = new();
        public string? description;
        public bool? spoiler;
    }

    public class ThumbnailBuilder: Thumbnail
    {
        public ThumbnailBuilder(string url, string? description = null, bool spoiler = false)
        {
            media.url = url;
            this.description = description;
            this.spoiler = spoiler;
        }

        public ThumbnailBuilder SetDescription(string description)
        {
            this.description = description;
            return this;
        }

        public ThumbnailBuilder SetSpoiler(bool spoiler)
        {
            this.spoiler = spoiler;
            return this;
        }
    }

    public class MediaGallery(): Component(ComponentType.MediaGallery)
    {
        public List<MediaGalleryItem> items = [];
    }

    public class MediaGalleryBuilder: MediaGallery
    {
        public MediaGalleryBuilder(params MediaGalleryItem[] items)
        {
            this.items = [.. items];
        }

        public MediaGalleryBuilder AddItem(params MediaGalleryItem[] items)
        {
            this.items.AddRange(items);
            return this;
        }

        public MediaGalleryBuilder SetItems(List<MediaGalleryItem> items)
        {
            this.items = items;
            return this;
        }
    }

    public class MediaGalleryItem
    {
        public UnfurledMediaItem media = new();
        public string? description;
        public bool? spoiler;
    }

    public class File(): Component(ComponentType.File)
    {
        public UnfurledMediaItem file = new();
        public bool? spoiler;
    }

    public class FileBuilder: File
    {
        public FileBuilder(string fileUrl)
        {
            file.url = fileUrl;
        }
    }

    public class UnfurledMediaItem
    {
        public string url = "attachment://<filename>";
        public string? proxy_url;
        public int? height;
        public int? width;
        public string? content_type;
    }

    public class Seperator(): Component(ComponentType.Seperator)
    {
        public bool? divider;
        public SeperatorSpacing? spacing;
    }

    public class SeperatorBuilder: Seperator
    {
        public SeperatorBuilder(bool showDivider = true, SeperatorSpacing dividerSpacing = SeperatorSpacing.Small)
        {
            divider = showDivider;
            spacing = dividerSpacing;
        }

        public SeperatorBuilder SetSpacing(SeperatorSpacing spacing)
        {
            this.spacing = spacing;
            return this;
        }
        public SeperatorBuilder ShowLine()
        {
            divider = true;
            return this;
        }
        public SeperatorBuilder HideLine()
        {
            divider = false;
            return this;
        }
    }

    public enum SeperatorSpacing
    {
        Small = 1,
        Large = 2
    }

    public class Container() : Component(ComponentType.Container)
    {
        [JsonProperty(ItemConverterType = typeof(ComponentConverter))]
        public List<Component> components = [];
        public int? accent_color;
        public Color accent
        {
            get => (Color)(accent_color ?? 0);
            set => accent_color = value.ToInt();
        }
        public bool? spoiler;
    }

    public class ContainerBuilder: Container
    {
        public ContainerBuilder() { }
        public ContainerBuilder(params Component[] component) {
            components = [..component];
        }

        public ContainerBuilder AddComponent(params Component[] component) 
        {
            components.AddRange(component);
            return this;
        }

        public ContainerBuilder SetComponents(List<Component> components)
        {
            this.components = components;
            return this;
        }

        public ContainerBuilder SetAccentColor(Color color)
        {
            accent = color;
            return this;
        }

        public ContainerBuilder SetAccentColor(int color)
        {
            accent_color = color;
            return this;
        }
    }

    public class Label(): Component(ComponentType.Label)
    {
        public string label = "";
        public string? description;
        [JsonConverter(typeof(ComponentConverter))]
        public Component component = new(ComponentType.StringSelect);
    }

    public class LabelBuilder: Label
    {
        public LabelBuilder() { }
        public LabelBuilder(string label, Component component)
        {
            this.label = label;
            this.component = component;
        }

        public LabelBuilder SetLabel(string label)
        {
            this.label = label;
            return this;
        }

        public LabelBuilder SetDescription(string description)
        {
            this.description = description;
            return this;
        }

        public LabelBuilder SetComponent(Component component)
        {
            this.component = component;
            return this;
        }
    }

    public class FileUpload(): Component(ComponentType.FileUpload)
    {
        public int? min_values;
        public int? max_values;
        public bool? required;
    }

    public class FileUploadBuilder: FileUpload
    {
        public FileUploadBuilder() { }
        public FileUploadBuilder SetMinValues(int minValues)
        {
            min_values = minValues;
            return this;
        }
        public FileUploadBuilder SetMaxValues(int maxValues)
        {
            max_values = maxValues;
            return this;
        }
        public FileUploadBuilder SetRequired(bool required)
        {
            this.required = required;
            return this;
        }
    }

    public enum ComponentType
    {
        ActionRow = 1,
        Button,
        StringSelect,
        TextInput,
        UserSelect,
        RoleSelect,
        MentionableSelect,
        ChannelSelect,
        Section,
        TextDisplay,
        Thumbnail,
        MediaGallery,
        File,
        Seperator,
        Container = 17,
        Label = 18,
        FileUpload = 19
    }
}

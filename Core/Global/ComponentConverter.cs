using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace YellowMacaroni.Discord.Core
{
    public class ComponentConverter : JsonConverter<Component>
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override Component? ReadJson(JsonReader reader, Type objectType, Component? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            
            // Try to get the component type, preferring 'type' over 'component_type'
            var typeToken = obj["type"] ?? obj["component_type"];
            if (typeToken == null)
            {
                return new Component(ComponentType.ActionRow); // Default fallback
            }

            var componentType = (ComponentType)typeToken.Value<int>();

            Component component = componentType switch
            {
                ComponentType.ActionRow => new ActionRow(),
                ComponentType.Button => new Button(),
                ComponentType.StringSelect => new StringSelect(),
                ComponentType.TextInput => new TextInput(),
                ComponentType.UserSelect => new UserSelect(),
                ComponentType.RoleSelect => new RoleSelect(),
                ComponentType.MentionableSelect => new MentionableSelect(),
                ComponentType.ChannelSelect => new ChannelSelect(),
                ComponentType.Section => new Section(),
                ComponentType.TextDisplay => new TextDisplay(),
                ComponentType.Thumbnail => new Thumbnail(),
                ComponentType.MediaGallery => new MediaGallery(),
                ComponentType.File => new File(),
                ComponentType.Seperator => new Seperator(),
                ComponentType.Container => new Container(),
                ComponentType.Label => new Label(),
                ComponentType.FileUpload => new FileUpload(),
                _ => new Component(componentType)
            };

            // Use the serializer to populate the component with the JSON data
            serializer.Populate(obj.CreateReader(), component);
            return component;
        }

        public override void WriteJson(JsonWriter writer, Component? value, JsonSerializer serializer)
        {
            // We don't need custom write logic, use default serialization
            throw new NotImplementedException("Use default serialization.");
        }
    }
}
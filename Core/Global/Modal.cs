using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class Modal
    {
        public string custom_id = "";
        public string title = "";
        [JsonProperty(ItemConverterType = typeof(ComponentConverter))]
        public List<Component> components = [];
    }

    public class ModalBuilder: Modal
    {
        public ModalBuilder SetCustomId(string customId)
        {
            custom_id = customId;
            return this;
        }

        public ModalBuilder SetTitle(string titleText)
        {
            title = titleText;
            return this;
        }

        public ModalBuilder AddComponent(params Component[] component)
        {
            components ??= [];
            components.AddRange(component);
            return this;
        }
    }
}

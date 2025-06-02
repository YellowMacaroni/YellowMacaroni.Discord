using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class Embed
    {
        /// <summary>
        /// The title of the embed.
        /// </summary>
        public string? title;

        /// <summary>
        /// The type of embed, does not need to be changed.
        /// </summary>
        public string? type = "rich";

        /// <summary>
        /// The description of the embed.
        /// </summary>
        public string? description;

        /// <summary>
        /// The URL of the embed which will be opened when the title is clicked.
        /// </summary>
        public string? url;

        /// <summary>
        /// The ISO8601 timestamp of the embed content.
        /// </summary>
        public string? timestamp;

        /// <summary>
        /// The color code of the embed.
        /// </summary>
        public int? color;

        /// <summary>
        /// The <see cref="color"/> as a <see cref="Core.Color"/> object.
        /// </summary>
        public Color Color
        {
            get => (Color)(color ?? 0);
            set => color = (int)value;
        }

        /// <summary>
        /// The embed's footer.
        /// </summary>
        public EmbedFooter? footer;

        /// <summary>
        /// The embed's image.
        /// </summary>
        public EmbedImage? image;

        /// <summary>
        /// The embed's thumbnail.
        /// </summary>
        public EmbedThumbnail? thumbnail;

        /// <summary>
        /// The embed's video.
        /// </summary>
        public EmbedVideo? video;

        /// <summary>
        /// The embed's provider.
        /// </summary>
        public EmbedProvider? provider;

        /// <summary>
        /// The embed's author.
        /// </summary>
        public EmbedAuthor? author;

        /// <summary>
        /// The embed's fields.
        /// </summary>
        public List<EmbedField>? fields;

        /// <summary>
        /// Convert the embed into an <see cref="EmbedBuilder"/>.
        /// </summary>
        public EmbedBuilder ToBuilder ()
        {
            return new EmbedBuilder(this);
        }
    }

    public class EmbedBuilder: Embed
    {
        public EmbedBuilder () { }

        public EmbedBuilder (Embed embed)
        {
            foreach (var prop in embed.GetType().GetProperties())
            {
                var value = prop.GetValue(embed);
                if (value is not null)
                {
                    prop.SetValue(this, value);
                }
            }
        }
        
        /// <summary>
        /// Add a title to the embed.
        /// </summary>
        /// <param name="title">The title of the embed.</param>
        /// <returns></returns>
        public EmbedBuilder SetTitle (string title)
        {
            this.title = title;
            return this;
        }

        /// <summary>
        /// Add a description to the embed.
        /// </summary>
        /// <param name="description">The description of the embed.</param>
        /// <returns></returns>
        public EmbedBuilder SetDescription (string description)
        {
            this.description = description;
            return this;
        }

        /// <summary>
        /// Set the URL for the title of the embed.
        /// </summary>
        /// <param name="url">The URL for the embed.</param>
        /// <returns></returns>
        public EmbedBuilder SetUrl (string url)
        {
            this.url = url;
            return this;
        }

        /// <summary>
        /// Set the color of the embed's accent.
        /// </summary>
        /// <param name="color">The RGB color to use.</param>
        /// <returns></returns>
        public EmbedBuilder SetColor (Color color)
        {
            this.Color = color;
            return this;
        }

        /// <summary>
        /// Set the color of the embed's accent.
        /// </summary>
        /// <param name="color">The hex value to use (as a number).</param>
        /// <returns></returns>
        public EmbedBuilder SetColor (int color)
        {
            this.color = color;
            return this;
        }

        /// <summary>
        /// Set the footer of the embed.
        /// </summary>
        /// <param name="footer">The footer for the embed.</param>
        /// <returns></returns>
        public EmbedBuilder SetFooter (EmbedFooter footer)
        {
            this.footer = footer;
            return this;
        }

        /// <summary>
        /// Set the footer of the embed.
        /// </summary>
        /// <param name="text">The text to use in the footer.</param>
        /// <param name="icon_url">The URL of the icon to use in the footer.</param>
        /// <returns></returns>
        public EmbedBuilder SetFooter (string text, string? icon_url = null)
        {
            this.footer = new EmbedFooter
            {
                text = text,
                icon_url = icon_url
            };
            return this;
        }

        /// <summary>
        /// Set the main image to use in the embed.
        /// </summary>
        /// <param name="url">The URL of the image to use.</param>
        /// <returns></returns>
        public EmbedBuilder SetImage (string url)
        {
            this.image = new EmbedImage
            {
                url = url
            };
            return this;
        }

        /// <summary>
        /// Set the thumbnail image to use in the embed.
        /// </summary>
        /// <param name="url">The URL of the image to use.</param>
        /// <returns></returns>
        public EmbedBuilder SetThumbnail (string url)
        {
            this.thumbnail = new EmbedThumbnail
            {
                url = url
            };
            return this;
        }

        /// <summary>
        /// Set the author of the embed.
        /// </summary>
        /// <param name="author">The author of the embed.</param>
        /// <returns></returns>
        public EmbedBuilder SetAuthor(EmbedAuthor author)
        {
            this.author = author;
            return this;
        }

        /// <summary>
        /// Set the author of the embed.
        /// </summary>
        /// <param name="name">The name of the author.</param>
        /// <param name="icon_url">The URL for the image of the author.</param>
        /// <param name="url">The URL to go to when the author is clicked on.</param>
        /// <returns></returns>
        public EmbedBuilder SetAuthor(string name, string? icon_url = null, string? url = null)
        {
            this.author = new EmbedAuthor
            {
                name = name,
                url = url,
                icon_url = icon_url
            };
            return this;
        }

        /// <summary>
        /// Add fields to the embed.
        /// </summary>
        /// <param name="field">The field to add.</param>
        /// <returns></returns>
        public EmbedBuilder AddField (params EmbedField[] field)
        {
            fields ??= [];
            fields.AddRange(field);
            return this;
        }

        /// <summary>
        /// Add a field to the embed.
        /// </summary>
        /// <param name="name">The name/title of the field.</param>
        /// <param name="value">The content of the field.</param>
        /// <param name="inline">Whether the field is inline with other inline fields.</param>
        /// <returns></returns>
        public EmbedBuilder AddField (string name, string value, bool? inline = false)
        {
            fields ??= [];
            fields.Add(new EmbedField
            {
                name = name,
                value = value,
                inline = inline
            });
            return this;
        }
    }

    public class EmbedFooter
    {
        /// <summary>
        /// The main text of the footer.
        /// </summary>
        public string? text;

        /// <summary>
        /// The URL to the footer's icon (only supports http(s) and attachments).
        /// </summary>
        public string? icon_url;

        /// <summary>
        /// The URL to the footer's icon (only supports http(s) and attachments).
        /// </summary>
        public string? proxy_icon_url;
    }

    public class EmbedImage
    {
        /// <summary>
        /// The URL of the image (only supports http(s) and attachments).
        /// </summary>
        public string? url;

        /// <summary>
        /// The URL of the image (only supports http(s) and attachments).
        /// </summary>
        public string? proxy_url;

        /// <summary>
        /// The height of the image.
        /// </summary>
        public int? height;

        /// <summary>
        /// The width of the image.
        /// </summary>
        public int? width;
    }

    public class EmbedThumbnail
    {
        /// <summary>
        /// The URL of the thumbnail (only supports http(s) and attachments).
        /// </summary>
        public string? url;

        /// <summary>
        /// The URL of the thumbnail (only supports http(s) and attachments).
        /// </summary>
        public string? proxy_url;

        /// <summary>
        /// The height of the thumbnail.
        /// </summary>
        public int? height;

        /// <summary>
        /// The width of the thumbnail.
        /// </summary>
        public int? width;
    }

    public class EmbedVideo
    {
        /// <summary>
        /// The URL of the video (only supports http(s) and attachments).
        /// </summary>
        public string? url;

        /// <summary>
        /// The URL of the video (only supports http(s) and attachments).
        /// </summary>
        public string? proxy_url;

        /// <summary>
        /// The height of the video.
        /// </summary>
        public int? height;

        /// <summary>
        /// The width of the video.
        /// </summary>
        public int? width;
    }

    public class EmbedProvider
    {
        /// <summary>
        /// The name of the provider.
        /// </summary>
        public string? name;

        /// <summary>
        /// The URL of the provider.
        /// </summary>
        public string? url;
    }

    public class EmbedAuthor
    {
        /// <summary>
        /// The name of the author.
        /// </summary>
        public string? name;

        /// <summary>
        /// The URL of the author.
        /// </summary>
        public string? url;

        /// <summary>
        /// The URL of the author's icon (only supports http(s) and attachments).
        /// </summary>
        public string? icon_url;

        /// <summary>
        /// The URL of the author's icon (only supports http(s) and attachments).
        /// </summary>
        public string? proxy_icon_url;
    }

    public class EmbedField
    {
        /// <summary>
        /// The name of the field.
        /// </summary>
        public string? name;

        /// <summary>
        /// The value of the field.
        /// </summary>
        public string? value;

        /// <summary>
        /// Whether the field should display inline.
        /// </summary>
        public bool? inline;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowMacaroni.Discord.Core
{
    public class Sticker (string stickerId)
    {
        /// <summary>
        /// The ID of the sticker.
        /// </summary>
        public string id = stickerId;

        /// <summary>
        /// The ID of the pack that the sticker is from (<see cref="StickerType.Standard"/> stickers only).
        /// </summary>
        public string? pack_id;

        /// <summary>
        /// The name of the sticker.
        /// </summary>
        public string? name;

        /// <summary>
        /// The description of the sticker.
        /// </summary>
        public string? description;

        /// <summary>
        /// Autocomplete/suggestion tags for the sticker (max 200 characters).
        /// </summary>
        public string? tags;

        /// <summary>
        /// The type of sticker as a <see cref="StickerType"/> enum.
        /// </summary>
        public StickerType? type;

        /// <summary>
        /// Whether this <see cref="StickerType.Guild"/> sticker can be used, may be false due to a loss of server boosts.
        /// </summary>
        public bool? available;

        /// <summary>
        /// The ID of the guild that owns the sticker.
        /// </summary>
        public string? guild_id;

        /// <summary>
        /// The user that uploaded the guild sticker.
        /// </summary>
        public User? user;

        /// <summary>
        /// The standard sticker's sort order within its pack.
        /// </summary>
        public int? sort_value;
    }

    public class StickerItem
    {
        public string? id;
        public string? name;
        public StickerFormatType? format_type;
    }

    public enum StickerType
    {
        Standard = 1,
        Guild = 2
    }

    public enum StickerFormatType
    {
        PNG = 1,
        APNG = 2,
        LOTTIE = 3,
        GIF = 4
    }
}

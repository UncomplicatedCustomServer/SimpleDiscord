using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDiscord.Components
{
#nullable enable
    public class Embed(string? title, string? type, string? description, string? url, string? timestamp, int? color, EmbedFooter? footer, EmbedResource? image, EmbedResource? thumbnail, EmbedResource? video, EmbedAuthor? author, EmbedField[]? fields)
    {
        public string? Title { get; } = title;

        public string? Type { get; } = type;

        public string? Description { get; } = description;

        public string? Url { get; } = url;

        public string? Timestamp { get; } = timestamp;

        public int? Color { get; } = color;

        public EmbedFooter? Footer { get; } = footer;

        public EmbedResource? Image { get; } = image;

        public EmbedResource? Thumbnail { get; } = thumbnail;

        public EmbedResource? Video { get; } = video;

        public EmbedAuthor? Author { get; } = author;

        public EmbedField[]? Fields { get; } = fields;

        public static Embed Build(string title, string description, string? url = null, int? color = null, EmbedField[]? fields = null) => new(title, "rich", description, url, null, color, null, null, null, null, null, fields);
    }
}

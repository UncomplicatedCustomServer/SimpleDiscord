﻿namespace SimpleDiscord.Components
{
#nullable enable
    public class Embed(string? title, string? type, string? description, string? url, string? timestamp, int? color, EmbedFooter? footer, EmbedResource? image, EmbedResource? thumbnail, EmbedResource? video, EmbedAuthor? author, EmbedField[]? fields)
    {
        public string? Title { get; internal set; } = title;

        public string? Type { get; internal set; } = type;

        public string? Description { get; internal set; } = description;

        public string? Url { get; internal set; } = url;

        public string? Timestamp { get; internal set; } = timestamp;

        public int? Color { get; internal set; } = color;

        public EmbedFooter? Footer { get; internal set; } = footer;

        public EmbedResource? Image { get; internal set; } = image;

        public EmbedResource? Thumbnail { get; internal set; } = thumbnail;

        public EmbedResource? Video { get; internal set; } = video;

        public EmbedAuthor? Author { get; internal set; } = author;

        public EmbedField[]? Fields { get; internal set; } = fields;

        internal Embed() : this(null, null, null, null, null, null, null, null, null, null, null, null) { }

        public static Embed Build(string title, string description, string? url = null, int? color = null, EmbedField[]? fields = null) => new(title, "rich", description, url, null, color, null, null, null, null, null, fields);
    }
}

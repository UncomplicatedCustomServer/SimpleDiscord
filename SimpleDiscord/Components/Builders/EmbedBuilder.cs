using System;
using System.Globalization;
using System.Linq;

namespace SimpleDiscord.Components.Builders
{
    public class EmbedBuilder()
    {
        private Embed Embed { get; set; } = new();

        public EmbedBuilder SetTitle(string title)
        {
            Embed.Title = title;
            return this;
        }

        public EmbedBuilder SetDescription(string description) 
        {
            Embed.Description = description; 
            return this; 
        }

#nullable enable
        public EmbedBuilder SetAuthor(string name, string? url = null, string? iconUrl = null)
        {
            Embed.Author = new(name, url, iconUrl);
            return this;
        }

        public EmbedBuilder SetFooter(string text, string? iconUrl)
        {
            Embed.Footer = new(text, iconUrl);
            return this;
        }

        public EmbedBuilder SetThumbnail(string url, int? width, int? height)
        {
            Embed.Thumbnail = new(url, null, height, width);
            return this;
        }

        public EmbedBuilder SetImage(string url, int? width, int? height)
        {
            Embed.Image = new(url, null, height, width);
            return this;
        }

        public EmbedBuilder SetTimestamp(DateTimeOffset? offset = null)
        {
            offset ??= DateTimeOffset.Now.ToLocalTime();

            Embed.Timestamp = ((DateTimeOffset)offset).ToString("s", CultureInfo.InvariantCulture);
            return this;
        }

        public EmbedBuilder AddField(string name, string value, bool? inline = false)
        {
            Embed.Fields.Append(new(name, value, inline));
            return this;
        }
#nullable disable

        public EmbedBuilder SetAuthor(EmbedAuthor author)
        {
            Embed.Author = author;
            return this;
        }

        public EmbedBuilder SetFooter(EmbedFooter footer)
        {
            Embed.Footer = footer;
            return this;
        }

        public EmbedBuilder SetThumbnail(EmbedResource thumbnail)
        {
            Embed.Thumbnail = thumbnail;
            return this;
        }

        public EmbedBuilder SetImage(EmbedResource image)
        {
            Embed.Image = image;
            return this;
        }

        public EmbedBuilder SetColor(string hex)
        {
            Embed.Color = int.Parse(hex.Replace("#", string.Empty), System.Globalization.NumberStyles.HexNumber);
            return this;
        }

        public EmbedBuilder SetColor(int color)
        {
            Embed.Color = color;
            return this;
        }

        public EmbedBuilder SetType(string type)
        {
            Embed.Type = type;
            return this;
        }

        public EmbedBuilder SetUrl(string url)
        {
            Embed.Url = url;
            return this;
        }

        public EmbedBuilder AddField(EmbedField field)
        {
            Embed.Fields.Append(field);
            return this;
        }

        public static EmbedBuilder New() => new();

        public static implicit operator Embed(EmbedBuilder builder) => builder.Embed;
    }
}

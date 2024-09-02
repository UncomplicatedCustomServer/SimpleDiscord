using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDiscord.Components
{
#nullable enable
    public class Embed
    {
        public string? Title { get; }

        public string? Type { get; } = "rich";

        public string? Description { get; }

        public string? Url { get; }

        public string? Timestamp { get; }

        public int? Color { get; }

        public EmbedFooter? Footer { get; }

        public EmbedResource? Image { get; }

        public EmbedResource? Thumbnail { get; }

        public EmbedResource? Video { get; }

        public EmbedAuthor? Author { get; }

        public EmbedField[]? Fields { get; }
    }
}

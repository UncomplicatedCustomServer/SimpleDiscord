namespace SimpleDiscord.Components
{
#nullable enable
    public class EmbedAuthor(string name, string? url, string? iconUrl, string? proxyUrl = null)
    {
        public string Name { get; } = name;

        public string? Url { get; } = url;

        public string? IconUrl { get; } = iconUrl;

        public string? ProxyUrl { get; } = proxyUrl;
    }
}

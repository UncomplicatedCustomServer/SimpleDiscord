namespace SimpleDiscord.Components
{
#nullable enable
    public class EmbedFooter(string text, string? iconUrl = null, string? proxyIconUrl = null)
    {
        public string Text { get; } = text;

        public string? IconUrl { get; } = iconUrl;

        public string? ProxyIconUrl { get; } = proxyIconUrl;
    }
}

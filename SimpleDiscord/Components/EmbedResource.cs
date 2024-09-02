namespace SimpleDiscord.Components
{
#nullable enable
    public class EmbedResource(string url, string? proxyUrl = null, int? height = null, int? width = null)
    {
        public string Url { get; } = url;

        public string? ProxyUrl { get; } = proxyUrl;

        public int? Height { get; } = height;

        public int? Width { get; } = width;
    }
}

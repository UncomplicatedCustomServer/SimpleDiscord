namespace SimpleDiscord.Components
{
#nullable enable
    public class Attachment(long id, string filename, string title, string description, string contentType, int size, string url, string proxyUrl) : PartialAttachment(id)
    {
        public string Filename { get; } = filename;

        public string? Title { get; } = title;

        public string? Description { get; } = description;

        public string? ContentType { get; } = contentType;

        public int Size { get; } = size;

        public string Url { get; } = url;

        public string ProxyUrl { get; } = proxyUrl;
    }
}

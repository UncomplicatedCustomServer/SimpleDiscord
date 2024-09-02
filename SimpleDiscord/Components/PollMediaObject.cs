namespace SimpleDiscord.Components
{
#nullable enable
    public class PollMediaObject(string? text = null, string? emoji = null)
    {
        public string? Text { get; } = text;

        public string? Emoji { get; } = emoji;
    }
}

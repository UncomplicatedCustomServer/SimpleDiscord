namespace SimpleDiscord.Components
{
    public class ActivityButton(string label, string url)
    {
        public string Label { get; } = label;

        public string Url { get; } = url;
    }
}

namespace SimpleDiscord.Components
{
#nullable enable
    public class ActivityAsset
    {
        public ActivityAsset(string largeImage, string largeText, string? smallImage = null, string? smallText = null)
        {
            LargeImage = largeImage;
            LargeText = largeText;
            SmallImage = smallImage ?? LargeImage;
            SmallText = smallText ?? LargeText;
        }

        public string LargeImage { get; set; }

        public string LargeText { get; set; }

        public string SmallImage { get; set; }

        public string SmallText { get; set; }
    }
}

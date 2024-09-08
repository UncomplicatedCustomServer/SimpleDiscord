namespace SimpleDiscord.Components
{
    public class SocketSendPublicThread(string name, int? autoArchiveDuration = 1440, int? rateLimitPerUser = null)
    {
        public string Name { get; set; } = name;

        public int? AutoArchiveDuration { get; set; } = autoArchiveDuration;

        public int? RateLimitPerUser { get; set; } = rateLimitPerUser;
    }
}

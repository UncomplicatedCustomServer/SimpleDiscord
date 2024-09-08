namespace SimpleDiscord.Components
{
    public class SocketSendCustomizableThread(string name, int? autoArchiveDuration = 1440, int? rateLimitPerUser = null, int? type = null, bool? invitable = null) : SocketSendPublicThread(name, autoArchiveDuration, rateLimitPerUser)
    {
        public int? Type { get; set; } = type;

        public bool? Invitable { get; set; } = invitable;
    }
}

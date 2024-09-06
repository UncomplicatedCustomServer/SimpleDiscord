namespace SimpleDiscord.Components
{
    public class SocketSendPresence(Activity[] activities, string status, bool afk = false, long? since = null)
    {
        public long? Since { get; } = since;

        public Activity[] Activities { get; } = activities;

        public string Status { get; } = status;

        public bool Afk { get; } = afk;
    }
}

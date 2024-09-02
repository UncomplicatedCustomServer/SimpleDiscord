namespace SimpleDiscord.Components.Communicator
{
    public class ReadyFields(int v, SocketUser user, UnavailableGuild[] guilds, string sessionId, string resumeGatewayUrl)
    {
        public int V { get; } = v;

        public SocketUser User { get; } = user;

        public UnavailableGuild[] Guilds { get; } = guilds;

        public string SessionId { get; } = sessionId;

        public string ResumeGatewayUrl { get; } = resumeGatewayUrl;
    }
}

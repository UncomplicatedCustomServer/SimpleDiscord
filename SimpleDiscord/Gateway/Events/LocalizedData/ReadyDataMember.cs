using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
#nullable enable
    internal class ReadyDataMember(int v, SocketUser user, UnavailableGuild[] guilds, string sessionId, string resumeGatewayUrl, int[]? shard, Application application)
    {
        public int V { get; } = v;

        public SocketUser User { get; } = user;

        public UnavailableGuild[] Guilds { get; } = guilds;

        public string SessionId { get; } = sessionId;

        public string ResumeGatewayUrl { get; } = resumeGatewayUrl;

        public int[]? Shard { get; } = shard;

        public PartialApplication Application { get; } = application;
    }
}

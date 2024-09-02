namespace SimpleDiscord.Components
{
#nullable enable

    public class SocketUser(long id, string username, string discriminator, string? globalName = null, string? avatar = null)
    {
        public long Id { get; } = id;

        public string Username { get; } = username;

        public string Discriminator { get; } = discriminator;

        public string? GlobalName { get; } = globalName;

        public string? Avatar { get; } = avatar;
    }
}

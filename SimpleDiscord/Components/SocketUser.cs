using System.Collections.Generic;
using System.Linq;

namespace SimpleDiscord.Components
{
#nullable enable

    public class SocketUser
    {
        public static readonly List<SocketUser> List = [];

        public long Id { get; }

        public string Username { get; }

        public string Discriminator { get; }

        public string? GlobalName { get; }

        public string? Avatar { get; }

        public SocketUser(long id, string username, string discriminator, string? globalName = null, string? avatar = null)
        {
            Id = id;
            Username = username;
            Discriminator = discriminator;
            GlobalName = globalName;
            Avatar = avatar;

            SocketUser instance = List.FirstOrDefault(u => u.Id == Id);
            if (instance is not null)
                List[List.IndexOf(instance)] = this;
            else
                List.Add(this);
        }
    }
}

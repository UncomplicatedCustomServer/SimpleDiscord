namespace SimpleDiscord.Components
{
#nullable enable
    public class Emoji(long? id, string name, Role[]? roles = null, SocketUser? user = null, bool? requireColons = null, bool? managed = null, bool? animated = null, bool? available = null)
    {
        public long? Id { get; } = id;

        public string Name { get; } = name;

        public Role[]? Roles { get; } = roles;

        public SocketUser? User { get; } = user;

        public bool? RequireColons { get; } = requireColons;

        public bool? Managed { get; } = managed;

        public bool? Animated { get; } = animated;

        public bool? Avaialble { get; } = available;

        public string Encode() => Id is null ? Name : $"{Name}:{Id}";

        public override string ToString() => Id is null ? Name : $"<{Name}:{Id}>";
    }
}

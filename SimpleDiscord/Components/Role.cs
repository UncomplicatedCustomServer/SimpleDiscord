namespace SimpleDiscord.Components
{
#nullable enable
    public class Role(long id, string name, int color, bool hoist, string? icon, string? unicodeEmoji, int position, string permissions, bool managed, bool mentionable, int flags)
    {
        public long Id { get; } = id;

        public string Name { get; } = name;

        public int Color { get; } = color;

        public bool Hoist { get; } = hoist;

        public string? Icon { get; } = icon;

        public string? UnicodeEmoji { get; } = unicodeEmoji;

        public int Position { get; } = position;

        public string Permissions { get; } = permissions;

        public bool Managed { get; } = managed;

        public bool Mentionable { get; } = mentionable;

        public int Flags { get; } = flags;

        public override string ToString() => $"<@&{Id}>";

        public override bool Equals(object obj) => obj is Role role ? Id == role.Id : base.Equals(obj);
    }
}

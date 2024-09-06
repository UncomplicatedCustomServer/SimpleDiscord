namespace SimpleDiscord.Components
{
    public class SocketSendRole(string name, string permissions, int color, bool hoist, bool mentionable)
    {
        public string Name { get; } = name;

        public string Permissions { get; set; } = permissions;

        public int Color { get; } = color;

        public bool Hoist { get; } = hoist;

        public bool Mentionable { get; } = mentionable;

        public int? Icon => null;

        public int? UnicodeEmoji => null;
    }
}

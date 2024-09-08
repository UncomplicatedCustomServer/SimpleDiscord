using Newtonsoft.Json;
using SimpleDiscord.Enums;

namespace SimpleDiscord.Components.DiscordComponents
{
    public class SocketActionRow : GenericComponent
    {
        public override int Type => (int)ComponentType.ActionRow;

        public object[] Components { get; }

        [JsonConstructor]
        public SocketActionRow(object[] baseComponents)
        {
            Components = baseComponents;
        }

        public SocketActionRow(SocketActionRow self) : this(self.Components)
        { }

        public SocketActionRow(ActionRow child) : this([.. child.Components])
        { }
    }
}

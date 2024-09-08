using SimpleDiscord.Components.Attributes;
using SimpleDiscord.Components.DiscordComponents;

namespace SimpleDiscord.Components
{
#nullable enable
    [SocketInstance(typeof(SocketMessageComponentInteractionData))]
    public class MessageComponentInteractionData : InteractionData
    {
        public string CustomId { get; }

        public int ComponentType { get; }

        public SelectOption[]? Values { get; }

        public ResolvedData? Resolved { get; }

        public MessageComponentInteractionData(string customId, int componentType, SelectOption[]? values, ResolvedData? resolved)
        {
            CustomId = customId;
            ComponentType = componentType;
            Values = values;
            Resolved = resolved;
        }
        
        public MessageComponentInteractionData(MessageComponentInteractionData self)
        {
            CustomId = self.CustomId;
            ComponentType = self.ComponentType;
            Values = self.Values;
            Resolved = self.Resolved;
        }

        public MessageComponentInteractionData(SocketMessageComponentInteractionData socketInstance) : this(socketInstance.CustomId, socketInstance.ComponentType, socketInstance.Values, socketInstance.Resolved is null ? null : new(socketInstance.Resolved)) 
        { }
    }
}

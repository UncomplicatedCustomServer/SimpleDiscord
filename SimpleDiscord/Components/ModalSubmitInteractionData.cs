using SimpleDiscord.Components.Attributes;
using SimpleDiscord.Components.DiscordComponents;

namespace SimpleDiscord.Components
{
#nullable enable
    [SocketInstance(typeof(SocketModalSubmitInteractionData))]
    public class ModalSubmitInteractionData(string customId, TextInput[]? components) : InteractionData
    {
        public string CustomId { get; } = customId;

        public TextInput[]? Components { get; } = components;

        public ModalSubmitInteractionData(ModalSubmitInteractionData self) : this(self.CustomId, self.Components)
        { }

        public ModalSubmitInteractionData(SocketModalSubmitInteractionData socketInstance) : this(socketInstance.CustomId, socketInstance.Components)
        { }
    }
}

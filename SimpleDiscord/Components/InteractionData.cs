using Newtonsoft.Json.Linq;
using SimpleDiscord.Enums;

namespace SimpleDiscord.Components
{
    public class InteractionData : SocketInteractionData
    {
        public static InteractionData Caster(Interaction interaction, JObject obj)
        {
            InteractionData data = interaction.Type switch
            {
                InteractionType.APPLICATION_COMMAND or InteractionType.APPLICATION_COMMAND_AUTOCOMPLETE => new ApplicationCommandInteractionData(obj.ToObject<SocketApplicationCommandInteractionData>()),
                InteractionType.MESSAGE_COMPONENT => new MessageComponentInteractionData(obj.ToObject<SocketMessageComponentInteractionData>()),
                InteractionType.MODAL_SUBMIT => new ModalSubmitInteractionData(obj.ToObject<SocketModalSubmitInteractionData>()),
                _ => null,
            };

            return data;
        }
    }
}

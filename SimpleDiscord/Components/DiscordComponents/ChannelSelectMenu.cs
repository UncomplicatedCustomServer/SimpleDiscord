using Newtonsoft.Json;
using SimpleDiscord.Enums;

namespace SimpleDiscord.Components.DiscordComponents
{
#nullable enable
    public class ChannelSelectMenu : SelectMenu
    {
        public override int Type => (int)ComponentType.ChannelSelect;

        public SelectDefaultValue[]? DefaultValues { get; }

        public ChannelType[]? ChannelTypes { get; }

        [JsonConstructor]
        public ChannelSelectMenu(SelectDefaultValue[]? defaultValues, ChannelType[]? channelTypes, string customId, string placeholder, int minValues, int maxValues, bool disabled = false) : base(customId, placeholder, minValues, maxValues, disabled)
        {
            DefaultValues = defaultValues;
            ChannelTypes = channelTypes;
        }

        public ChannelSelectMenu(SelectMenu socketSelectMenu, SelectDefaultValue[]? defaultValues, ChannelType[]? channelTypes) : base(socketSelectMenu)
        {
            DefaultValues = defaultValues;
            ChannelTypes = channelTypes;
        }

        public ChannelSelectMenu(ChannelSelectMenu self) : this(self, self.DefaultValues, self.ChannelTypes)
        { }
    }
}

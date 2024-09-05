using Newtonsoft.Json;
using SimpleDiscord.Enums;

namespace SimpleDiscord.Components.DiscordComponents
{
#nullable enable
    public class MentionableSelectMenu : SelectMenu
    {
        public override int Type => (int)ComponentType.MentionableSelect;

        public SelectDefaultValue[]? DefaultValues { get; }

        [JsonConstructor]
        public MentionableSelectMenu(SelectDefaultValue[]? defaultValues, string customId, string placeholder, int minValues, int maxValues, bool disabled = false) : base(customId, placeholder, minValues, maxValues, disabled)
        {
            DefaultValues = defaultValues;
        }

        public MentionableSelectMenu(SelectMenu socketSelectMenu, SelectDefaultValue[]? defaultValues) : base(socketSelectMenu)
        {
            DefaultValues = defaultValues;
        }

        public MentionableSelectMenu(MentionableSelectMenu self) : this(self, self.DefaultValues)
        { }
    }
}

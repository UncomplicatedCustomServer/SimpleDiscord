using Newtonsoft.Json;
using SimpleDiscord.Enums;

namespace SimpleDiscord.Components.DiscordComponents
{
    public class TextSelectMenu : SelectMenu
    {
        public override int Type => (int)ComponentType.StringSelect;

        public SelectOption[] Options { get; }

        [JsonConstructor]
        public TextSelectMenu(SelectOption[] options, string customId, string placeholder, int minValues, int maxValues, bool disabled = false) : base(customId, placeholder, minValues, maxValues, disabled)
        {
            Options = options;
        }

        public TextSelectMenu(SelectMenu partial, SelectOption[] options) : base(partial)
        {
            Options = options;
        }

        public TextSelectMenu(TextSelectMenu self) : this(self, self.Options)
        { }
    }
}

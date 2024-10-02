using Newtonsoft.Json;
using SimpleDiscord.Enums;

namespace SimpleDiscord.Components.DiscordComponents
{
#nullable enable
    public class UserSelectMenu : SelectMenu
    {
        public override int Type => (int)ComponentType.UserSelect;

        public SelectDefaultValue[]? DefaultValues { get; internal set; }

        [JsonConstructor]
        public UserSelectMenu(SelectDefaultValue[]? defaultValues, string customId, string placeholder, int minValues, int maxValues, bool disabled = false) : base(customId, placeholder, minValues, maxValues, disabled)
        {
            DefaultValues = defaultValues;
        }

        public UserSelectMenu(SelectMenu socketSelectMenu, SelectDefaultValue[]? defaultValues) : base(socketSelectMenu)
        {
            DefaultValues = defaultValues;
        }

        public UserSelectMenu(UserSelectMenu self) : this(self, self.DefaultValues)
        { }
    }
}

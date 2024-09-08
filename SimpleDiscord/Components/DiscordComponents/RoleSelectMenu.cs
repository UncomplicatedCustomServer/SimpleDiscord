using Newtonsoft.Json;
using SimpleDiscord.Enums;

namespace SimpleDiscord.Components.DiscordComponents
{
#nullable enable
    public class RoleSelectMenu : SelectMenu
    {
        public override int Type => (int)ComponentType.RoleSelect;

        public SelectDefaultValue[]? DefaultValues { get; }

        [JsonConstructor]
        public RoleSelectMenu(SelectDefaultValue[]? defaultValues, string customId, string placeholder, int minValues, int maxValues, bool disabled = false) : base(customId, placeholder, minValues, maxValues, disabled)
        {
            DefaultValues = defaultValues;
        }

        public RoleSelectMenu(SelectMenu socketSelectMenu, SelectDefaultValue[]? defaultValues) : base(socketSelectMenu)
        {
            DefaultValues = defaultValues;
        }

        public RoleSelectMenu(RoleSelectMenu self) : this(self, self.DefaultValues)
        { }
    }
}

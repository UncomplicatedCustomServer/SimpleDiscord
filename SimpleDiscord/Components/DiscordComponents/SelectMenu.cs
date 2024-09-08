using Newtonsoft.Json;

namespace SimpleDiscord.Components.DiscordComponents
{
    public abstract class SelectMenu : GenericComponent
    {
        public string CustomId { get; }
        
        public string Placeholder { get; }

        public int MinValues { get; }

        public int MaxValues { get; }

        public bool Disabled { get; }

        [JsonConstructor]
        protected SelectMenu(string customId, string placeholder, int minValues, int maxValues, bool disabled = false)
        {
            CustomId = customId;
            Placeholder = placeholder;
            MinValues = minValues;
            MaxValues = maxValues;
            Disabled = disabled;
        }

        protected SelectMenu(SelectMenu self)
        {
            CustomId = self.CustomId;
            Placeholder = self.Placeholder;
            MinValues = self.MinValues;
            MaxValues = self.MaxValues;
            Disabled = self.Disabled;
        }
    }
}

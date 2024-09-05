using System.Collections.Generic;

namespace SimpleDiscord.Components.DiscordComponents
{
    public class Modal : InteractionCallbackData
    {
        public string CustomId { get; internal set; }

        public string Title { get; internal set; }

        public List<ActionRow> Components { get; internal set; }

        public Modal(string customId, string title, TextInput[] components)
        {
            CustomId = customId;
            Title = title;
            Components = [new()];
            Components[0].BulkAdd(components);
        }

        internal Modal()
        {
            Components = [new()];
        }
    }
}

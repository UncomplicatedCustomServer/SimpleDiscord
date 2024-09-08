using SimpleDiscord.Components.DiscordComponents;
using System;

namespace SimpleDiscord.Components.Builders
{
    public class ActionRowBuilder()
    {
        private ActionRow ActionRow { get; } = new();

        public ActionRowBuilder AddComponent(GenericComponent component)
        {
            if (ActionRow.Components.Count >= 5)
                throw new Exception("Max component number reached! (5)");

            ActionRow.Components.Add(component);
            return this;
        }

        public static ActionRowBuilder New() => new();

        public static implicit operator SocketActionRow(ActionRowBuilder builder) => new(builder.ActionRow);
        public static implicit operator ActionRow(ActionRowBuilder builder) => builder.ActionRow;
    }
}

using SimpleDiscord.Components.DiscordComponents;
using SimpleDiscord.Enums;
using System;
using System.Linq;

namespace SimpleDiscord.Components.Builders
{
    public class ButtonBuilder
    {
        internal readonly Button button = new((int)ButtonStyle.Primary, null, null, RandomString(35), null);

        public ButtonBuilder SetLabel(string label)
        {
            button.Label = label;
            return this;
        }

        public ButtonBuilder SetStyle(ButtonStyle style)
        {
            button.Style = (int)style;
            return this;
        }

        public ButtonBuilder SetEmoji(Emoji emoji)
        {
            button.Emoji = emoji;
            return this;
        }

        public ButtonBuilder SetData(object data)
        {
            button.Data = data;
            return this;
        }

        public ButtonBuilder SetCallback(Action<object> callback)
        {
            button.Callback = callback;
            return this;
        }

        public ButtonBuilder SetCustomId(string customId)
        {
            button.CustomId = customId;
            return this;
        }

        public ButtonBuilder New() => new();

        public static implicit operator Button(ButtonBuilder builder) => builder.button;

        private static string RandomString(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz.,;_#@=()$?";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[Discord.Random.Next(s.Length)]).ToArray());
        }
    }
}

using SimpleDiscord.Components.DiscordComponents;
using SimpleDiscord.Enums;
using System.Linq;

namespace SimpleDiscord.Components.Builders
{
    public class ModalBuilder
    {
        private Modal Modal { get; set; } = new();

        public ModalBuilder SetTitle(string title)
        {
            Modal.Title = title;
            return this;
        }

        public ModalBuilder SetCustomId(string customId)
        {
            Modal.CustomId = customId;
            return this;
        }

        public ModalBuilder AddComponent(TextInput component)
        {
            Modal.Components[0].AddComponent(component);
            return this;
        }

#nullable enable
        public ModalBuilder AddComponent(string label, TextInputStyle style, string customId, int? minLength = null, int? maxLength = null, bool? required = null, string? value = null, string? placeholder = null)
        {
            Modal.Components[0].AddComponent(new TextInput(customId, style, label, minLength, maxLength, required, value, placeholder));
            return this;
        }
#nullable disable

        private bool Validate() => Modal.Title is not null && Modal.CustomId is not null && Modal.Components.Count <= 5 && Modal.Components.Count > 0;

        public static ModalBuilder New() => new();

        public static implicit operator Modal(ModalBuilder builder) => builder.Validate() ? builder.Modal : throw new System.Exception($"Failed to validate builder.\nParams {builder.Modal.Title is not null}, {builder.Modal.CustomId is not null}, {builder.Modal.Components.Count}");
    }
}

using Newtonsoft.Json.Linq;
using SimpleDiscord.Enums;

namespace SimpleDiscord.Components.DiscordComponents
{
    public abstract class GenericComponent
    {
        public abstract int Type { get; }

        public static GenericComponent Caster(ComponentType type, JObject obj)
        {
            GenericComponent component = type switch
            {
                ComponentType.ActionRow => obj.ToObject<SocketActionRow>(),
                ComponentType.Button => obj.ToObject<Button>(),
                ComponentType.TextInput => obj.ToObject<TextInput>(),
                ComponentType.StringSelect => obj.ToObject<TextSelectMenu>(),
                ComponentType.RoleSelect => obj.ToObject<RoleSelectMenu>(),
                ComponentType.UserSelect => obj.ToObject<UserSelectMenu>(),
                ComponentType.MentionableSelect => obj.ToObject<MentionableSelectMenu>(),
                ComponentType.ChannelSelect => obj.ToObject<ChannelSelectMenu>(),
                _ => null
            };

            return component;
        }
    }
}

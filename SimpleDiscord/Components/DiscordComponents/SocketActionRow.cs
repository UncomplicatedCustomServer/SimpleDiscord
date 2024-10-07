using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleDiscord.Enums;
using System;
using System.Collections.Generic;

namespace SimpleDiscord.Components.DiscordComponents
{
#nullable enable
    public class SocketActionRow : GenericComponent
    {
        public int? Id { get; }

        public override int Type { get; }

        public List<object>? Components { get; }

        public SocketActionRow(SocketActionRow self) : this((int)ComponentType.ActionRow, self.Components, self.Id)
        { }

        public SocketActionRow(ActionRow child) : this((int)ComponentType.ActionRow, [.. child.Components], child.Id)
        { }

        [JsonConstructor]
        public SocketActionRow(int type, List<object>? components, int? id = null)
        {
            Id = id;
            Type = type;
            Components = [];
            if (components is not null)
                foreach (object component in components)
                    if (component is JObject obj)
                        Components.Add(obj);
        }
    }
}

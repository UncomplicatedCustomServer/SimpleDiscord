using Newtonsoft.Json.Linq;
using SimpleDiscord.Components.Attributes;
using SimpleDiscord.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleDiscord.Components.DiscordComponents
{
    [SocketInstance(typeof(SocketActionRow))]
    public class ActionRow : SocketActionRow
    {
        public new List<GenericComponent> Components { get; } = [];

        public ActionRow() : base([]) => Components = [];

        public ActionRow(SocketActionRow actionRow) : base(actionRow)
        {
            if (actionRow.Components is not null)
                foreach (object component in actionRow.Components)
                    if (component is JObject obj)
                        Components.Add(Caster((ComponentType)Type, obj));
        }

        public SocketActionRow ToSocketInstance() => new([..Components]);

        public void BulkAdd(IEnumerable<GenericComponent> components)
        {
            if (components.Count() > 5)
                throw new Exception("Too many components!");

            foreach (GenericComponent component in components)
                Components.Add(component);
        }

        public void AddComponent(GenericComponent component) => Components.Add(component);
    }
}

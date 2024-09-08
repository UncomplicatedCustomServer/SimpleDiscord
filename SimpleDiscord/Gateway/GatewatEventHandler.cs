using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleDiscord.Gateway
{
    internal class GatewatEventHandler
    {
        private readonly List<EventIntents> _events = [];

        public GatewatEventHandler()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                object[] attribs = type.GetCustomAttributes(typeof(InternalEvent), false);
                if (attribs is not null && attribs.Length > 0)
                {
                    InternalEvent attribute = (InternalEvent)attribs[0];
                    _events.Add(new(type, attribute.OpCode, attribute.Event));
                }
            }
        }

        internal BaseGatewayEvent Parse(DiscordGatewayMessage message)
        {
            EventIntents instance = _events.FirstOrDefault(x => x.OpCode == message.OpCode && x.Event == message.EventName);

            if (instance is null)
                return null;

            BaseGatewayEvent ev = (BaseGatewayEvent)Activator.CreateInstance(instance.Type, [message]);
            ev.Init();
            return ev;
        }
    }
}

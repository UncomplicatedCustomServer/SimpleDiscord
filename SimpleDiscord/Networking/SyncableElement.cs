using Newtonsoft.Json.Linq;
using SimpleDiscord.Components;
using SimpleDiscord.Components.Attributes;
using System;
using System.Threading.Tasks;

namespace SimpleDiscord.Networking
{
#pragma warning disable CS8603
#nullable enable
    public class SyncableElement : ISyncableElement
    {
        public async Task<object>? Sync()
        {
            Type caller = GetType();

            object[] attr = caller.GetCustomAttributes(typeof(SocketInstance), false);
            if (attr is null || attr.Length is 0)
                return null; // ErrorManager

            Type socketInstance = ((SocketInstance)attr[0]).Instance;

            object[] attr2 = socketInstance.GetCustomAttributes(typeof(EndpointInfo), true);
            if (attr2 is null || attr2.Length is 0)
                return null; // ErrorManager

            string endpoint = ((EndpointInfo)attr2[0]).Endpoint;

            bool guildUpdate = false;
            Guild? guild = null;

            if (typeof(IGuildElement).IsAssignableFrom(caller))
            {
                guildUpdate = true;
                guild = (Guild)caller.GetProperty(nameof(IGuildElement.Guild)).GetValue(caller, null);
                endpoint.Replace("{guild.id}", guild.Id.ToString());
                if (caller == typeof(GuildChannel) || typeof(GuildChannel).IsAssignableFrom(caller))
                    endpoint.Replace("{channel.id}", (string)(caller.GetProperty(nameof(GuildChannel.Id)).GetValue(caller, null) ?? ""));

                if (caller == typeof(SocketMessage) || typeof(SocketMessage).IsAssignableFrom(caller))
                    endpoint.Replace("{message.id}", (string)(caller.GetProperty(nameof(SocketMessage.Id)).GetValue(caller, null) ?? ""));
            }

            string content = await Client.Instance.RestHttp.SendGenericGetRequest(endpoint);

            JObject obj = JObject.Parse(content);

            object? socketElement = obj.ToObject(socketInstance);

            if (socketElement is null)
                return null;

            object instance = Activator.CreateInstance(caller, [socketElement]);

            if (guildUpdate && guild is not null)
            {
                GuildChannel? channel = instance as GuildChannel;
                if (channel is not null)
                    guild.SafeUpdateChannel(channel);
            }

            return instance;
        }
    }
}

using Newtonsoft.Json.Linq;
using SimpleDiscord.Components;
using SimpleDiscord.Enums;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("GUILD_CREATE")]
    internal class GuildCreate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public AnonymousGuild Data { get; private set; }

        public Guild Guild { get; private set; }

        public override void Init()
        {
            JObject localData = JObject.Parse(RawData);
            Data = localData.ToObject<AnonymousGuild>();

            // Now we can update the channels
            Guild guild = Guild.List.FirstOrDefault(g => g.Id == Data.Id);

            if (guild is null)
                return;

            if (localData["channels"] is null)
            {
                Console.WriteLine($"Cannot find 'channels' inside guild object?");
                return;
            }

            foreach (JObject channel in localData["channels"].ToObject<List<JObject>>())
            {
                channel["guild_id"] = Data.Id;
                SocketGuildChannel parsedChannel = channel.ToObject<SocketGuildChannel>();
                switch ((ChannelType)parsedChannel.Type)
                {
                    case ChannelType.GUILD_TEXT or ChannelType.GUILD_ANNOUNCEMENT:
                        guild.SafeUpdateChannel(new GuildTextChannel(channel.ToObject<SocketGuildTextChannel>()));
                        break;
                    case ChannelType.GUILD_VOICE:
                        guild.SafeUpdateChannel(new GuildVoiceChannel(channel.ToObject<SocketGuildVoiceChannel>()));
                        break;
                    case ChannelType.PRIVATE_THREAD or ChannelType.PUBLIC_THREAD:
                        guild.SafeUpdateChannel(new GuildThreadChannel(channel.ToObject<SocketGuildThreadChannel>()));
                        break;
                    default:
                        guild.SafeUpdateChannel(new(parsedChannel));
                        break;
                }
            }

            if (localData["threads"] is not null)
                foreach (JObject thread in localData["threads"].ToObject<List<JObject>>())
                {
                    thread["guild_id"] = Data.Id;
                    guild.SafeUpdateThread(new GuildThreadChannel(thread.ToObject<SocketGuildThreadChannel>()));
                }

            Guild = guild;
        }
    }
}

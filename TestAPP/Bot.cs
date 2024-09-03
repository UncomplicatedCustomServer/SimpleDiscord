using SimpleDiscord.Events.Attribute;
using SimpleDiscord.Events;
using SimpleDiscord.Gateway;
using SimpleDiscord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAPP
{
    internal class Bot
    {
        public Client DiscordClient { get; }

        public Bot()
        {
            Handler.RegisterEvents(typeof(Bot).Assembly, this);
            DiscordClient = new();
        }

        [SocketEvent("READY")] 
        public void OnReady()
        {
            DiscordClient.Presence = new([new("MI PIACE LECCARE I FURRY", 0)], "dnd");
        }

        [SocketEvent("MESSAGE_CREATE")]
        public void OnMessage()
        {

        }

        public async Task Connect()
        {
            await DiscordClient.LoginAsync("token", Gateway.privilegedIntents);
        }
    }
}

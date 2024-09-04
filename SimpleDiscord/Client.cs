using SimpleDiscord.Components;
using SimpleDiscord.Enums;
using SimpleDiscord.Events;
using SimpleDiscord.Gateway.Events;
using SimpleDiscord.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleDiscord
{
#pragma warning disable IDE1006
    public class Client
    {
        public HashSet<Guild> Guilds => [.. Guild.List];

        public static Client Instance { get; private set; }

        public SocketPresence Presence { 
            get
            {
                return _actualPresence;
            }
            set
            {
                Console.WriteLine("\n\n\nPORCODIODSISI MAGIC MAFIZ!");
                _discordClient.SendMessage(new(string.Empty, 3, value, null, null));
                Console.WriteLine("\nMAGIC MAFIZ!");
                _actualPresence = value;
            }
        }

        private SocketPresence _actualPresence;

        private Discord _discordClient { get; }

        internal Http RestHttp { get; }

        internal string Token { get; private set; }

        public Client()
        {
            BaseGatewayEvent.InitEvents();
            _discordClient = new();
            RestHttp = new(_discordClient.httpClient);
            Instance = this;
        }

        public async Task LoginAsync(string token, GatewayIntents intents = Gateway.Gateway.defaultIntents)
        {
            Token = token;
            RestHttp.HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bot {token}");
            await _discordClient.AuthAsync(token, intents);
            Handler.Invoke("READY", null);
        }

        public void Disconnect()
        {
            _discordClient.Disconnect();
        }
      
#nullable enable
        public Guild? GetGuild(long id) => Guild.List.FirstOrDefault(guild => guild.Id == id);
#nullable disable

        internal Guild GetSafeGuild(long id) => Guild.List.FirstOrDefault(safeguild => safeguild.Id == id);
    }
}

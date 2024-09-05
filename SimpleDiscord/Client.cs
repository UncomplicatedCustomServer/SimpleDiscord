using SimpleDiscord.Components;
using SimpleDiscord.Enums;
using SimpleDiscord.Events;
using SimpleDiscord.Gateway;
using SimpleDiscord.Networking;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleDiscord
{
#pragma warning disable IDE1006
    public class Client
    {
        public bool SaveMessages { get; set; } = true;

        public bool ForceRegisterMessages { get; set; } = false;

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

        internal GatewatEventHandler GatewatEventHandler { get; }

        public Handler EventHandler { get; }

        internal Http RestHttp { get; }

        internal string Token { get; private set; }

        public Client()
        {
            GatewatEventHandler = new();
            EventHandler = new();
            _discordClient = new(this);
            RestHttp = new(_discordClient.httpClient);
        }

        public async Task LoginAsync(string token, GatewayIntents intents = Gateway.Gateway.defaultIntents)
        {
            Token = token;
            RestHttp.HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bot {token}");
            await _discordClient.AuthAsync(token, intents);
            EventHandler.Invoke("READY", null);
        }

        public void Disconnect() => _discordClient.Disconnect();

#nullable enable
        public Guild? GetGuild(long id) => Guild.List.FirstOrDefault(guild => guild.Id == id);
#nullable disable

        internal Guild GetSafeGuild(long id) => Guild.List.FirstOrDefault(safeguild => safeguild.Id == id);
    }
}

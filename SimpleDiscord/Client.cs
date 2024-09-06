﻿using SimpleDiscord.Components;
using SimpleDiscord.Enums;
using SimpleDiscord.Events;
using SimpleDiscord.Gateway;
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
        public readonly ClientConfig Config;

        public readonly Handler EventHandler;

        public SocketUser Bot { get; internal set; }

        public PartialApplication Application { get; internal set; }

        public List<ApplicationCommand> Commands { get; } = [];

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

        internal GatewatEventHandler GatewatEventHandler { get; }

        internal Http RestHttp { get; }

        internal string Token { get; private set; }

        internal string SessionId { get; set; }

        internal string ResumeGatewayUrl { get; set; }

        internal List<SocketSendApplicationCommand> sendCommandsQueue { get; set; } = [];

        private SocketPresence _actualPresence;

        private Discord _discordClient { get; }

        public Client(ClientConfig config = null)
        {
            Config = config ?? new();
            GatewatEventHandler = new();
            EventHandler = new();
            _discordClient = new(this);
            RestHttp = new(_discordClient.httpClient, this);
        }

        public async Task LoginAsync(string token, GatewayIntents intents = Gateway.Gateway.defaultIntents)
        {
            Token = token;
            RestHttp.HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bot {token}");
            await _discordClient.AuthAsync(token, intents); // Can't remove await: eventloop
        }

        public void Disconnect() => _discordClient.Disconnect();

        public async void RegisterCommand(SocketSendApplicationCommand command)
        {
            if (sendCommandsQueue is not null)
                sendCommandsQueue.Add(command);
            else
            {
                SocketApplicationCommand cmd = await RestHttp.CreateGlobalCommand(command);
                if (Config.SaveGlobalRegisteredCommands)
                    Commands.Add(new(cmd));
            }
        }

        public async Task<SocketApplicationCommand> EditCommand(ApplicationCommand command, SocketSendApplicationCommand newCommand) => await RestHttp.EditGlobalCommand(command, newCommand);

        public void DeleteCommand(ApplicationCommand command) => RestHttp.DeleteGlobalCommand(command);

#nullable enable
        public Guild? GetGuild(long id) => Guild.List.FirstOrDefault(guild => guild.Id == id);
#nullable disable

        internal Guild GetSafeGuild(long id) => Guild.List.FirstOrDefault(safeguild => safeguild.Id == id);
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SimpleDiscord.Components;
using SimpleDiscord.Enums;
using SimpleDiscord.Gateway.Events;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Messages;
using SimpleDiscord.Gateway.Messages.Predefined;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleDiscord
{
    internal class Discord(Client client)
    {
        public ConnectionStatus connectionStatus = ConnectionStatus.Ready;

        private string endpoint;

        internal ClientWebSocket webSocketClient = new();

        internal HttpClient httpClient = new();

        private readonly Client DiscordClient = client;

        internal static readonly Random Random = new();

        private uint? heartbeatDelay = null;

        // Kinda useless for how is structured the lib
        private int? lastSequence = null;

        private string token;

        private GatewayIntents intents;

        private bool _areadyAuthed = false;

        // It just contains the pool messages Id for results forced by us
        internal List<long> pollResults = [];

        // Just a Dictionary to hold every button callback (yeah we hate Modals and things like that) - Anyways the key is the CustomId while the values are the object data and the action
        internal readonly Dictionary<string, KeyValuePair<object, Action<object>>> buttonCallbacks = [];

        internal async Task AuthAsync(string token, GatewayIntents intents)
        {
            this.token = token;
            this.intents = intents;
            DiscordClient.Logger.Silent("Starting the Discord Client, requireing the Discord Gateway from the APIs");
            await RetriveEndpoint();
            DiscordClient.Logger.Silent($"Found Discord Gateway: {endpoint}");
            await Connect();
        }

        private async Task RetriveEndpoint()
        {
            if (endpoint != null && endpoint != string.Empty)
                return;

            if (httpClient is null)
                DiscordClient.Logger.Error("HTTP CLIENT IS NULL!");

            string endpointData = await httpClient.GetStringAsync("https://discord.com/api/gateway");

            if (endpointData is null)
            {
                endpointData = "{\"url\":\"wss://gateway.discord.gg\"}";
                DiscordClient.Logger.Error("Failed to retrive Discord gateway from APIs, using the default one...");
            }

            Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(endpointData);

            if (data.TryGetValue("url", out string url))
                endpoint = url + "?v=10&encoding=json";
            else
                DiscordClient.ErrorHub.Throw($"Got invalid response from Discord Gateway Hub!\nKilling process", true);
        }

        internal async Task Connect()
        {
            if (endpoint == null || endpoint == string.Empty)
                DiscordClient.ErrorHub.Throw($"Cannot connect to the given endpoint as it seems to be invalid!\nEndpoint: {endpoint}");

            await webSocketClient.ConnectAsync(new(endpoint), CancellationToken.None);

            DiscordClient.Logger.Silent($"Successfully enstabilished connection with the Discord Gateway, authenticating...");

            connectionStatus = ConnectionStatus.Connecting;

            MessageReceiver();
        }

        internal async void Disconnect() => await webSocketClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);

        internal async void MessageReceiver()
        {
            List<byte> received = [];
            while (connectionStatus is not ConnectionStatus.NotAvailable and not ConnectionStatus.NotConnected and not ConnectionStatus.Ready && webSocketClient.State is WebSocketState.Open)
            {
                if (webSocketClient.State is not WebSocketState.Open)
                {
                    DiscordClient.Logger.Warn("Lost connection with the Discord Gateway, reconnecting in 2 seconds...");
                    await Task.Delay(2200);
                    await Connect();
                }
                byte[] buffer = new byte[2048];
                WebSocketReceiveResult result = await webSocketClient.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                received.AddRange(buffer.Take(result.Count));

                if (result.MessageType is WebSocketMessageType.Close)
                {
                    DiscordClient.Logger.Error($"Connection to the Discord Gateway has closed!\nClose code: {result.CloseStatus} - {result.CloseStatusDescription}");
                    if (result.CloseStatus is WebSocketCloseStatus.EndpointUnavailable)
                    {
                        DiscordClient.Logger.Warn("Lost connection with the Discord Gateway, reconnecting in 2 seconds...");
                        await Task.Delay(2200);
                        await Connect();
                    }
                    return;
                }

                if (result.EndOfMessage)
                {
                    MessageHandler(Encoding.UTF8.GetString([.. received]));
                    received.Clear();
                }
            }
        }

        private void MessageHandler(string message)
        {
            DiscordRawGatewayMessage rawMsg = new(message, JsonConvert.DeserializeObject<Dictionary<string, object>>(message));

            if (rawMsg.S is not null)
                lastSequence = rawMsg.S;

            DiscordClient.EventHandler.Invoke("RAW_GATEWAY_EVENT", new DiscordGatewayMessage(rawMsg));

            BaseGatewayEvent ev = DiscordClient.GatewatEventHandler.Parse(new(rawMsg));

            if (ev is GuildCreate guildCreate)
            {
                guildCreate.Guild.SetClient(DiscordClient);
                Task.Run(guildCreate.Guild.SafeRegisterCommands); // Async plz

                // Register every user if needed
                if (DiscordClient.Config.LoadMembers)
                    SendMessage(new(string.Empty, 8, new GuildChunkMemberData(guildCreate.Guild.Id)));
            }

            if (ev is MessageCreate messageCreate && messageCreate.CanShare)
                messageCreate.Message.SetClient(DiscordClient);

            if (ev is MessageUpdate messageUpdate && messageUpdate.CanShare)
                messageUpdate.Message.SetClient(DiscordClient);

            if (ev is InteractionCreate interactionCreate)
            {
                interactionCreate.Interaction.SetClient(DiscordClient);
                if (interactionCreate.Interaction.Type is InteractionType.APPLICATION_COMMAND && interactionCreate.Interaction.Data is ApplicationCommandInteractionData data)
                    DiscordClient.EventHandler.InvokeCommand(data.Name, interactionCreate.Interaction, data);
                else if (interactionCreate.Interaction.Type is InteractionType.MESSAGE_COMPONENT && interactionCreate.Interaction.Data is MessageComponentInteractionData data2)
                {
                    if (data2.ComponentType is (int)ComponentType.Button && buttonCallbacks.TryGetValue(data2.CustomId, out KeyValuePair<object, Action<object>> callback))
                        callback.Value(callback.Key);

                    DiscordClient.EventHandler.InvokeComponent(data2.CustomId, data2);
                }
            }

            if (ev is Heartbeat)
            {
                SendHeartbeat();
                return;
            }

            if (ev is Ready ready)
            {
                DiscordClient.Logger.Silent("Client is ready!");
                DiscordClient.Application = ready.Data.Application;
                DiscordClient.Bot = ready.Data.User;
                DiscordClient.SessionId = ready.Data.SessionId;
                DiscordClient.ResumeGatewayUrl = ready.Data.ResumeGatewayUrl;

                if (DiscordClient.Config.LoadAppInfo)
                {
                    Task<Application> task = DiscordClient.RestHttp.GetCurrentApplication();
                    task.Wait();
                    DiscordClient.Application = task.Result;
                }
            }

            if (connectionStatus is ConnectionStatus.Connected or ConnectionStatus.Connecting && ev is not null && ev.GatewayMessage.EventName != null)
            {
                if (ev is IUserDeniableEvent deniableEvent && !deniableEvent.CanShare)
                    goto proceed;

                if (ev is MessageUpdate messageUpdated && pollResults.Contains(messageUpdated.Message.Id))
                {
                    DiscordClient.EventHandler.Invoke("POLL_ENDED", new PollEnded(messageUpdated.Message, messageUpdated.Message.Poll));
                    goto proceed;
                }

                DiscordClient.EventHandler.Invoke(ev.GatewayMessage.EventName, ev);
            }

        proceed:

            if (ev is Hello hello)
            {
                connectionStatus = ConnectionStatus.Identifying;
                heartbeatDelay = hello.Data.HeartbeatInterval;
                HeartbeatHandler();
            }
            else if (ev is HeartbeatAck)
            {
                if (_areadyAuthed)
                    DiscordClient.Logger.Silent("[HEARTBEAT] Got ACK!");
                else
                {
                    SendMessage(new(string.Empty, 2, new Identify(token, new()
                        {
                            {
                                "os",
                                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "windows" :
                                RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "linux" :
                                RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "osx" : 
                                "Unknown"
                            },
                            {
                                "browser",
                                "SimpleDiscord"
                            },
                            {
                                "device",
                                "SimpleDiscord"
                            }
                        }, (int)intents)));
                    _areadyAuthed = true;
                    connectionStatus = ConnectionStatus.Connecting;
                }
            }
            else if (ev is Ready)
            {
                connectionStatus = ConnectionStatus.Connected;
                Task.Run(RegisterGlobalCommands); // Register global commands with the respect of RL
            }
        }

        public async void RegisterGlobalCommands()
        {
            if (DiscordClient.Config.RegisterCommands is RegisterCommandType.None)
                return;

            await Task.Delay(2000);
            List<SocketApplicationCommand> globalCommands = [.. (await DiscordClient.RestHttp.GetGlobalCommands())];
            foreach (SocketSendApplicationCommand cmd in DiscordClient.sendCommandsQueue)
            {
                bool exists = globalCommands.FirstOrDefault(c => c.Name == cmd.Name) is not null;
                if (exists && DiscordClient.Config.RegisterCommands is not RegisterCommandType.CreateAndEdit)
                    continue;

                SocketApplicationCommand command = await DiscordClient.RestHttp.CreateGlobalCommand(cmd);
                DiscordClient.Logger.Info($"Successfully registered command {command.Name}!");
                globalCommands.Add(command);
                await Task.Delay(4250);
            }

            DiscordClient.sendCommandsQueue = null;

            if (DiscordClient.Config.SaveGlobalRegisteredCommands)
                foreach (SocketApplicationCommand cmd in globalCommands)
                    DiscordClient.Commands.Add(new(cmd));
        }

        internal void SendMessage(DiscordRawGatewayMessage raw)
        {
            DefaultContractResolver contractResolver = new()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            string data = JsonConvert.SerializeObject(raw, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

            webSocketClient.SendAsync(new(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Binary, true, CancellationToken.None);
        }

        private async void HeartbeatHandler()
        {
            while (connectionStatus is not ConnectionStatus.NotAvailable and not ConnectionStatus.NotConnected and not ConnectionStatus.Ready && heartbeatDelay is not null)
            {
                if (heartbeatDelay is 0)
                    return;

                DiscordClient.Logger.Silent("[HEARTBEAT] Sending heartbeat...");
                SendHeartbeat();
                int time = (int)heartbeatDelay + Random.Next((int)(heartbeatDelay * -1) / 4, -15);
                await Task.Delay(time);
            }
        }

        private void SendHeartbeat() => SendMessage(new(string.Empty, 1, null, lastSequence, null));
    }
}

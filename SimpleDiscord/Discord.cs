using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SimpleDiscord.Components;
using SimpleDiscord.Enums;
using SimpleDiscord.Events;
using SimpleDiscord.Gateway.Events;
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
#pragma warning disable IDE0052
    internal class Discord(Client client)
    {
        public ConnectionStatus connectionStatus = ConnectionStatus.Ready;

        private string endpoint;

        internal ClientWebSocket webSocketClient = new();

        internal HttpClient httpClient = new();

        private readonly Client DiscordClient = client;

        internal static readonly Random Random = new();

        private uint? heartbeatDelay = null;

        private int? lastSequence = null;

        private string token;

        private GatewayIntents intents;

        private bool _areadyAuthed = false;

        internal async Task AuthAsync(string token, GatewayIntents intents)
        {
            this.token = token;
            this.intents = intents;
            await RetriveEndpoint();
            Console.WriteLine($"\nFound endpoint: {endpoint}!");
            await Connect();
        }

        private async Task RetriveEndpoint()
        {
            if (endpoint != null && endpoint != string.Empty)
                return;

            Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(await httpClient.GetStringAsync("https://discord.com/api/gateway"));

            if (data.TryGetValue("url", out string url))
                endpoint = url + "?v=10&encoding=json";
            else
                throw new HttpRequestException("The answer from Discord API is not valid!");
        }

        internal async Task Connect()
        {
            if (endpoint == null || endpoint == string.Empty)
                throw new WebSocketException("Can't connect to the given URL: " + endpoint);

            await webSocketClient.ConnectAsync(new(endpoint), CancellationToken.None);

            connectionStatus = ConnectionStatus.Connecting;

            await MessageReceiver();
        }

        internal void Disconnect()
        {
            _ = webSocketClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None).ConfigureAwait(false);
        }

        internal async Task MessageReceiver()
        {
            Console.WriteLine($"Current status: {connectionStatus}");
            List<byte> received = [];
            while (connectionStatus is not ConnectionStatus.NotAvailable and not ConnectionStatus.NotConnected and not ConnectionStatus.Ready && webSocketClient.State is WebSocketState.Open)
            {
                Console.WriteLine("buffer!");
                byte[] buffer = new byte[2048];
                WebSocketReceiveResult result = await webSocketClient.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                received.AddRange(buffer.Take(result.Count));

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine($"\nConnection closed!\nStatus: {result.CloseStatus} - {result.CloseStatusDescription}");
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
            Console.WriteLine(message);

            Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(message);

            DiscordRawGatewayMessage rawMsg = new(message, data);

            if (rawMsg.S is not null)
                lastSequence = rawMsg.S;

            BaseGatewayEvent ev = DiscordClient.GatewatEventHandler.Parse(new(rawMsg));

            if (ev is GuildCreate guildCreate)
                guildCreate.Guild.SetClient(DiscordClient);

            if (ev is MessageCreate messageCreate)
                messageCreate.Message.SetClient(DiscordClient);

            if (ev is InteractionCreate interactionCreate)
                interactionCreate.Interaction.SetClient(DiscordClient);

            if (ev is Ready ready)
            {
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
                Console.WriteLine($"\nInvoking event {ev.GatewayMessage.EventName} ({ev.GatewayMessage.OpCode}) -- found {ev.GetType().FullName}\n");
                if (ev is IUserDeniableEvent deniableEvent && deniableEvent.CanShare)
                    goto proceed;

                DiscordClient.EventHandler.Invoke(ev.GatewayMessage.EventName, ev);
            }

        proceed:

            if (ev is Hello hello)
            {
                Console.WriteLine($"\nHELLO!\nDDL: {hello.Data.HeartbeatInterval}\n");
                connectionStatus = ConnectionStatus.Identifying;
                heartbeatDelay = hello.Data.HeartbeatInterval;
                HeartbeatHandler();
            }
            else if (ev is HeartbeatAck)
            {
                Console.WriteLine("\nACK!\n");
                if (_areadyAuthed)
                    Console.WriteLine("\nReceived ACK\n");
                else
                {
                    SendMessage(new(string.Empty, 2, new Identify(token, new()
                        {
                            {
                                "os",
                                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "windows" :
                                RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "linux" :
                                RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "osx" : "Unknown"
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
                    Console.Write((int)intents);
                    _areadyAuthed = true;
                    connectionStatus = ConnectionStatus.Connecting;
                }
            }
            else if (ev is Ready)
            {
                Console.WriteLine("\nREADY!\n");
                connectionStatus = ConnectionStatus.Connected;
                Task.Run(RegisterGlobalCommands); // Register global commands with the respect of RL
            }

            if (ev is null)
                Console.WriteLine("Event is not handled");
        }

        public async void RegisterGlobalCommands()
        {
            if (DiscordClient.Config.RegisterCommands is RegisterCommandType.None)
                return;

            await Task.Delay(1500);
            List<SocketApplicationCommand> globalCommands = [.. (await DiscordClient.RestHttp.GetGlobalCommands())];
            foreach (SocketSendApplicationCommand cmd in DiscordClient.sendCommandsQueue)
            {
                bool exists = globalCommands.FirstOrDefault(c => c.Name == cmd.Name) is not null;
                if (exists && DiscordClient.Config.RegisterCommands is not RegisterCommandType.CreateAndEdit)
                    continue;

                SocketApplicationCommand command = await DiscordClient.RestHttp.CreateGlobalCommand(cmd);
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
            try
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
                Console.WriteLine("\nSending info!\nData: " + data + "\n");

                webSocketClient.SendAsync(new(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Binary, true, CancellationToken.None);

                Console.WriteLine("Ok were here");
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }
        }

        private async void HeartbeatHandler()
        {
            while (connectionStatus is not ConnectionStatus.NotAvailable and not ConnectionStatus.NotConnected and not ConnectionStatus.Ready && heartbeatDelay is not null)
            {
                if (heartbeatDelay is 0)
                    return;

                Console.WriteLine("\nHeartbeat!\n");
                SendHeartbeat();
                int time = (int)heartbeatDelay + Random.Next(-50, 50);
                Console.WriteLine($"Waiting time: {time} -- {heartbeatDelay}");
                await Task.Delay(time);
            }
        }

        private void SendHeartbeat() => SendMessage(new(string.Empty, 1, null, lastSequence, null));
    }
}

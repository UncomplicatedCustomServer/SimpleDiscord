using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleDiscord
{
#pragma warning disable IDE1006
    internal class Discord
    {
        public ConnectionStatus connectionStatus = ConnectionStatus.Ready;

        private string endpoint;

        internal ClientWebSocket webSocketClient = new();

        internal HttpClient httpClient = new();

        private Random random = new();

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

            BaseGatewayEvent ev = BaseGatewayEvent.Parse(new(rawMsg));

            if (connectionStatus is ConnectionStatus.Connected or ConnectionStatus.Connecting && ev is not null && ev.GatewayMessage.EventName != null)
            {
                Console.WriteLine($"\nInvoking event {ev.GatewayMessage.EventName} ({ev.GatewayMessage.OpCode}) -- found {ev.GetType().FullName}\n");
                Handler.Invoke(ev.GatewayMessage.EventName, ev);
            }

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
                                "linux"
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
                Console.WriteLine("\nREADY!\n");
                connectionStatus = ConnectionStatus.Connected;
            }

            if (ev is null)
                Console.WriteLine("Event is not handled");
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
                if (heartbeatDelay == 0)
                    return;

                Console.WriteLine("\nHeartbeat!\n");
                SendHeartbeat();
                int time = (int)(heartbeatDelay * 1);
                Console.WriteLine($"Waiting time: {time} -- {heartbeatDelay}");
                await Task.Delay(time);
            }
        }

        private void SendHeartbeat() => SendMessage(new(string.Empty, 1, null, lastSequence, null));
    }
}

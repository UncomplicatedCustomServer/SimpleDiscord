using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SimpleDiscord.Components;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDiscord.Networking
{
    internal class Http(HttpClient client)
    {
        public HttpClient HttpClient { get; } = client;

        public const string Endpoint = "https://discord.com/api";

        public async Task<SocketMessage> SendMessage(SocketGuildTextChannel channel, SocketSendMessage message)
        {
            if (channel.Type is 4 or 14)
                throw new NotSupportedException("You can't send messages to a non-text channel!");

            Console.WriteLine($"\n\n{EncodeJson(message)} MAGIC\n\n{Endpoint}/channels/{channel.Id}/messages\n");

            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bot {Client.Instance.Token}");
            HttpResponseMessage answer = await HttpClient.PostAsync($"{Endpoint}/channels/{channel.Id}/messages", new StringContent(EncodeJson(message), Encoding.UTF8, "application/json"));
            string a = await answer.Content.ReadAsStringAsync();
            if (answer.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"Invalid answer: {answer.StatusCode} - {answer.ReasonPhrase}");

            return JsonConvert.DeserializeObject<SocketMessage>(await answer.Content.ReadAsStringAsync());
        }    
        
        private string EncodeJson(object data)
        {
            DefaultContractResolver contractResolver = new()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            return JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
        }
    }
}

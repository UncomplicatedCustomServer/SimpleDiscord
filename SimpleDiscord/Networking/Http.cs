using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SimpleDiscord.Components;
using SimpleDiscord.Components.DiscordComponents;
using SimpleDiscord.Enums;
using SimpleDiscord.Logger;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDiscord.Networking
{
    internal class Http(HttpClient client)
    {
        public HttpClient HttpClient { get; } = client;

        public const string Endpoint = "https://discord.com/api/v10";

        public async Task<string> SendGenericGetRequest(string url)
        {
            HttpResponseMessage answer = await HttpClient.GetAsync($"{Endpoint}{url}");
            if (answer.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"Invalid answer: {answer.StatusCode} - {answer.ReasonPhrase}");

            return await answer.Content.ReadAsStringAsync();
        }

        public async Task<SocketMessage> SendMessage(GuildTextChannel channel, SocketSendMessage message) => await SendMessage(new SocketGuildTextChannel(channel), message);

        public async Task<SocketMessage> SendMessage(SocketGuildTextChannel channel, SocketSendMessage message)
        {
            if (channel.Type is 4 or 14)
                throw new NotSupportedException("You can't send messages to a non-text channel!");

            Console.WriteLine($"\n\n{EncodeJson(message)} MAGIC\n\n{Endpoint}/channels/{channel.Id}/messages\n");

            HttpResponseMessage answer = await HttpClient.PostAsync($"{Endpoint}/channels/{channel.Id}/messages", new StringContent(EncodeJson(message), Encoding.UTF8, "application/json"));
            if (answer.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"Invalid answer: {answer.StatusCode} - {answer.ReasonPhrase}");

            return JsonConvert.DeserializeObject<SocketMessage>(await answer.Content.ReadAsStringAsync());
        }    

        public async Task<SocketMessage> EditMessage(SocketMessage originalMessage, SocketSendMessage message)
        {
            HttpResponseMessage answer = await HttpClient.SendAsync(new HttpRequestMessage()
            {
                RequestUri = new($"{Endpoint}/channels/{originalMessage.ChannelId}/messages/{originalMessage.Id}"),
                Method = new("PATCH"),
                Content = new StringContent(EncodeJson(message), Encoding.UTF8, "application/json")
            });

            if (answer.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"Invalid answer: {answer.StatusCode} - {answer.ReasonPhrase}");

            return JsonConvert.DeserializeObject<SocketMessage>(await answer.Content.ReadAsStringAsync());
        }

        public async Task<bool> DeleteMessage(SocketMessage message, string reason = null)
        {
            HttpRequestMessage httpMessage = new()
            {
                RequestUri = new($"{Endpoint}/channels/{message.ChannelId}/messages/{message.Id}"),
                Method = HttpMethod.Delete,
                Content = new StringContent(EncodeJson(message), Encoding.UTF8, "application/json"),
            };

            if (reason is not null)
                httpMessage.Headers.TryAddWithoutValidation("X-Audit-Log-Reason", reason);

            HttpResponseMessage answer = await HttpClient.SendAsync(httpMessage);

            if (answer.StatusCode != HttpStatusCode.NoContent)
                throw new HttpRequestException($"Invalid answer: {answer.StatusCode} - {answer.ReasonPhrase}");

            return true;
        }

        public async Task<SocketUser[]> GetReactions(Message message, string emoji, ReactionType type = ReactionType.NORMAL, int limit = 25, long? after = null)
        {
            string query = $"?type={(int)type}&limit={limit}";
            if (after is not null)
                query += $"&after={after}";

            if (limit > 100)
                throw new ArgumentOutOfRangeException("Limit cannot be greater than 100!");

            HttpResponseMessage answer = await HttpClient.GetAsync($"{Endpoint}/channels/{message.ChannelId}/messages/{message.Id}/reactions/{emoji}{query}");

            if (answer.StatusCode != HttpStatusCode.NoContent)
                throw new HttpRequestException($"Invalid answer: {answer.StatusCode} - {answer.ReasonPhrase}");

            return JsonConvert.DeserializeObject<SocketUser[]>(await answer.Content.ReadAsStringAsync());
        }
        
        public async Task<InteractionResponse> SendInteractionReply(Interaction interaction, InteractionResponse response)
        {
            HttpResponseMessage answer = await HttpClient.PostAsync($"{Endpoint}/interactions/{interaction.Id}/{interaction.Token}/callback?with_response=true", new StringContent(EncodeJson(response.ToSocketInstance()), Encoding.UTF8, "application/json"));

            string a = await answer.Content.ReadAsStringAsync();

            Log.Debug($"MSG: {EncodeJson(response.ToSocketInstance())}");

            if (answer.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"Invalid answer: {answer.StatusCode} - {answer.ReasonPhrase} - {a}");

            SocketInteractionResponse socketInteraction = JsonConvert.DeserializeObject<SocketInteractionResponse>(await answer.Content.ReadAsStringAsync());
            interaction.SafeUpdateResponse(socketInteraction);
            return new(socketInteraction);
        }

        public async Task<InteractionResponse> EditInteractionReply(Interaction interaction, InteractionResponse response)
        {
            HttpResponseMessage answer = await HttpClient.SendAsync(new HttpRequestMessage()
            {
                RequestUri = new($"{Endpoint}/webhooks/{interaction.ApplicationId}/{interaction.Token}/messages/@original"),
                Method = new("PATCH"),
                Content = new StringContent(EncodeJson(response.ToSocketInstance().Data), Encoding.UTF8, "application/json")
            });

            string a = await answer.Content.ReadAsStringAsync();

            if (answer.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"Invalid answer: {answer.StatusCode} - {answer.ReasonPhrase}: {a}");

            SocketInteractionResponse socketInteraction = JsonConvert.DeserializeObject<SocketInteractionResponse>(await answer.Content.ReadAsStringAsync());
            interaction.SafeUpdateResponse(socketInteraction);
            return new(socketInteraction);
        }

        public async void DeleteInteractionReply(Interaction interaction)
        {
            HttpResponseMessage answer = await HttpClient.DeleteAsync($"{Endpoint}/interactions/{interaction.ApplicationId}/{interaction.Token}/messages/@original");

            if (answer.StatusCode != HttpStatusCode.NoContent)
                throw new HttpRequestException($"Invalid answer: {answer.StatusCode} - {answer.ReasonPhrase}");

            interaction.ClearResponse();
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

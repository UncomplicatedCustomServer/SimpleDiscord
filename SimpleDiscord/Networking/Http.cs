using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SimpleDiscord.Components;
using SimpleDiscord.Components.Builders;
using SimpleDiscord.Components.DiscordComponents;
using SimpleDiscord.Enums;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleDiscord.Networking
{
    internal class Http(HttpClient client, Client originalClient)
    {
        public HttpClient HttpClient { get; } = client;

        private readonly RateLimitHandler rateLimitHandler = new();

        private readonly Client discordClient = originalClient;

        public const string Endpoint = "https://discord.com/api/v10";

        public async Task<string> SendGenericGetRequest(string url)
        {
            HttpResponseMessage answer = await HttpClient.GetAsync($"{Endpoint}{url}");
            if (answer.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"Invalid answer: {answer.StatusCode} - {answer.ReasonPhrase}");

            return await answer.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> Send(HttpRequestMessage message, HttpStatusCode expectedResponse = HttpStatusCode.OK, bool disableResponseCheck = false)
        {
            RateLimit rateLimit = rateLimitHandler.GetRateLimit(message);
            if (rateLimit is not null)
                if (!rateLimit.Validate())
                    await Task.Delay((int)(rateLimit.EnqueueTime() * 1000)); // Enqueued

            HttpResponseMessage answer = await HttpClient.SendAsync(message);
            rateLimitHandler.UpdateRateLimit(message, answer.Headers);

            if (!disableResponseCheck && answer.StatusCode != expectedResponse)
                throw new Exception($"Failed to send a HTTP {message.Method.Method} request to {message.RequestUri.OriginalString}.\nExpected OK (200), got {answer.StatusCode} ({(int)answer.StatusCode})");

            return answer;
        }

        public async Task<SocketMessage> SendMessage(GuildTextChannel channel, SocketSendMessage message)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Post).SetUri($"{Endpoint}/channels/{channel.Id}/messages").SetJsonContent(EncodeJson(message)));

            return JsonConvert.DeserializeObject<SocketMessage>(await answer.Content.ReadAsStringAsync());
        }    

        public async Task<SocketMessage> EditMessage(SocketMessage originalMessage, SocketSendMessage message)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod("PATCH").SetUri($"{Endpoint}/channels/{originalMessage.ChannelId}/messages/{originalMessage.Id}").SetJsonContent(EncodeJson(message)));

            return JsonConvert.DeserializeObject<SocketMessage>(await answer.Content.ReadAsStringAsync());
        }

        public async Task<bool> DeleteMessage(SocketMessage message, string reason = null)
        {
            HttpMessageBuilder builder = HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/channels/{message.ChannelId}/messages/{message.Id}").SetJsonContent(EncodeJson(message));

            if (reason is not null)
                builder.SetHeader("X-Audit-Log-Reason", reason);

            await Send(builder);

            return true;
        }

        public async Task<SocketUser[]> GetReactions(Message message, string emoji, ReactionType type = ReactionType.NORMAL, int limit = 25, long? after = null)
        {
            string query = $"?type={(int)type}&limit={limit}";
            if (after is not null)
                query += $"&after={after}";

            if (limit > 100)
                throw new ArgumentOutOfRangeException("Limit cannot be greater than 100!");

            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Get).SetUri($"{Endpoint}/channels/{message.ChannelId}/messages/{message.Id}/reactions/{emoji}{query}").SetJsonContent(EncodeJson(message)));

            return JsonConvert.DeserializeObject<SocketUser[]>(await answer.Content.ReadAsStringAsync());
        }
        
        public async Task<InteractionResponse> SendInteractionReply(Interaction interaction, InteractionResponse response)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Post).SetUri($"{Endpoint}/interactions/{interaction.Id}/{interaction.Token}/callback?with_response=true").SetJsonContent(EncodeJson(response.ToSocketInstance())));

            SocketInteractionResponse socketInteraction = JsonConvert.DeserializeObject<SocketInteractionResponse>(await answer.Content.ReadAsStringAsync());
            interaction.SafeUpdateResponse(socketInteraction);
            return new(socketInteraction);
        }

        public async Task<InteractionResponse> EditInteractionReply(Interaction interaction, InteractionResponse response)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod("PATCH").SetUri($"{Endpoint}/webhooks/{interaction.ApplicationId}/{interaction.Token}/messages/@original").SetJsonContent(EncodeJson(response.ToSocketInstance())));

            SocketInteractionResponse socketInteraction = JsonConvert.DeserializeObject<SocketInteractionResponse>(await answer.Content.ReadAsStringAsync());
            interaction.SafeUpdateResponse(socketInteraction);
            return new(socketInteraction);
        }

        public async void DeleteInteractionReply(Interaction interaction)
        {
            await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/interactions/{interaction.ApplicationId}/{interaction.Token}/messages/@original"));

            interaction.ClearResponse();
        }

        public async Task<Application> GetCurrentApplication()
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Get).SetUri($"{Endpoint}/applications/@me"));

            return JsonConvert.DeserializeObject<Application>(await answer.Content.ReadAsStringAsync());
        } 

        public async Task<SocketApplicationCommand[]> GetGlobalCommands()
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Get).SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/commands"));

            return JsonConvert.DeserializeObject<SocketApplicationCommand[]>(await answer.Content.ReadAsStringAsync());
        }

        public async Task<SocketApplicationCommand> CreateGlobalCommand(SocketSendApplicationCommand command)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Post).SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/commands").SetJsonContent(EncodeJson(command)), disableResponseCheck:true);

            if (answer.StatusCode is not HttpStatusCode.OK and HttpStatusCode.Created)
                throw new Exception($"Failed to send a HTTP request!.\nExpected OK (200), got {answer.StatusCode} ({(int)answer.StatusCode})");

            return JsonConvert.DeserializeObject<SocketApplicationCommand>(await answer.Content.ReadAsStringAsync());
        }

        public async Task<SocketApplicationCommand> EditGlobalCommand(ApplicationCommand command, SocketSendApplicationCommand newCommand)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod("PATCH").SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/commands/{command.Id}").SetJsonContent(EncodeJson(newCommand)));

            return JsonConvert.DeserializeObject<SocketApplicationCommand>(await answer.Content.ReadAsStringAsync());
        }

        public async void DeleteGlobalCommand(ApplicationCommand command) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/commands/{command.Id}"));

        public async Task<SocketApplicationCommand[]> BulkOverwriteGlobalCommands(SocketApplicationCommand[] commands)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Put).SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/commands").SetJsonContent(EncodeJson(commands)));

            return JsonConvert.DeserializeObject<SocketApplicationCommand[]>(await answer.Content.ReadAsStringAsync());
        }

        public async Task<SocketApplicationCommand[]> GetGuildCommands(Guild guild)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Get).SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/guilds/{guild.Id}/commands"));

            return JsonConvert.DeserializeObject<SocketApplicationCommand[]>(await answer.Content.ReadAsStringAsync());
        }

        public async Task<SocketApplicationCommand> CreateGuildCommand(Guild guild, SocketSendApplicationCommand command)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Post).SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/guilds/{guild.Id}/commands").SetJsonContent(EncodeJson(command)), disableResponseCheck: true);

            if (answer.StatusCode is not HttpStatusCode.OK or HttpStatusCode.Created)
                throw new Exception($"Failed to send a HTTP request!.\nExpected OK (200), got {answer.StatusCode} ({(int)answer.StatusCode})");

            return JsonConvert.DeserializeObject<SocketApplicationCommand>(await answer.Content.ReadAsStringAsync());
        }

        public async Task<SocketApplicationCommand> EditGuildCommand(Guild guild, ApplicationCommand command, SocketSendApplicationCommand newCommand)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod("PATCH").SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/guilds/{guild.Id}/commands/{command.Id}").SetJsonContent(EncodeJson(newCommand)));

            return JsonConvert.DeserializeObject<SocketApplicationCommand>(await answer.Content.ReadAsStringAsync());
        }

        public async void DeleteGuildCommand(Guild guild, ApplicationCommand command) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/guilds/{guild.Id}/commands/{command.Id}"));

        public async Task<SocketApplicationCommand[]> BulkOverwriteGuildCommands(Guild guild, SocketApplicationCommand[] commands)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Put).SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/guilds/{guild.Id}/commands").SetJsonContent(EncodeJson(commands)));

            return JsonConvert.DeserializeObject<SocketApplicationCommand[]>(await answer.Content.ReadAsStringAsync());
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

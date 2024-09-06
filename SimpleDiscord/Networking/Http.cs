using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SimpleDiscord.Components;
using SimpleDiscord.Components.Builders;
using SimpleDiscord.Components.DiscordComponents;
using SimpleDiscord.Enums;
using SimpleDiscord.Logger;
using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Security.Authentication.ExtendedProtection;
using System.Threading.Tasks;

namespace SimpleDiscord.Networking
{
    internal class Http(HttpClient client, Client originalClient)
    {
        public HttpClient HttpClient { get; } = client;

        private readonly RateLimitHandler rateLimitHandler = new();

        private readonly Client discordClient = originalClient;

        public const string Endpoint = "https://discord.com/api/v10";

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

            if (answer.StatusCode is not HttpStatusCode.OK and not HttpStatusCode.Created)
                throw new Exception($"Failed to send a HTTP request!.\nExpected OK (200), got {answer.StatusCode} ({(int)answer.StatusCode})");

            return JsonConvert.DeserializeObject<SocketApplicationCommand>(await answer.Content.ReadAsStringAsync());
        }

        public async Task<SocketApplicationCommand> EditGlobalCommand(ApplicationCommand command, SocketSendApplicationCommand newCommand)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod("PATCH").SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/commands/{command.Id}").SetJsonContent(EncodeJson(newCommand)));

            return JsonConvert.DeserializeObject<SocketApplicationCommand>(await answer.Content.ReadAsStringAsync());
        }

        public async void DeleteGlobalCommand(ApplicationCommand command) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/commands/{command.Id}"), HttpStatusCode.NoContent);

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

        public async void DeleteGuildCommand(Guild guild, ApplicationCommand command) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/guilds/{guild.Id}/commands/{command.Id}"), HttpStatusCode.NoContent);

        public async Task<SocketApplicationCommand[]> BulkOverwriteGuildCommands(Guild guild, SocketApplicationCommand[] commands)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Put).SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/guilds/{guild.Id}/commands").SetJsonContent(EncodeJson(commands)));

            return JsonConvert.DeserializeObject<SocketApplicationCommand[]>(await answer.Content.ReadAsStringAsync());
        }

        public void MemberAddRole(Member member, Role role, string reason = null) => MemberAddRole(member, role.Id, reason);

        public async void MemberAddRole(Member member, long role, string reason = null)
        {
            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod(HttpMethod.Put).SetUri($"{Endpoint}/guilds/{member.Guild.Id}/members/{member.User.Id}/roles/{role}");

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            await Send(message, HttpStatusCode.NoContent);
        }

        public void MemberRemoveRole(Member member, Role role, string reason = null) => MemberRemoveRole(member, role.Id, reason);

        public async void MemberRemoveRole(Member member, long role, string reason = null)
        {
            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/guilds/{member.Guild.Id}/members/{member.User.Id}/roles/{role}");

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            await Send(message, HttpStatusCode.NoContent);
        }

        public async void MemberKick(Member member, string reason = null)
        {
            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/guilds/{member.Guild.Id}/members/{member.User.Id}");

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            await Send(message, HttpStatusCode.NoContent);
        }

        public async void MemberBan(Member member, string reason = null, int deleteMessageSeconds = 0)
        {
            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod(HttpMethod.Put).SetUri($"{Endpoint}/guilds/{member.Guild.Id}/bans/{member.User.Id}?delete_message_seconds={deleteMessageSeconds}");

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            await Send(message, HttpStatusCode.NoContent);
        }

        public async void MemberUnban(Guild guild, long userId, string reason = null)
        {
            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/guilds/{guild.Id}/bans/{userId}");

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            await Send(message, HttpStatusCode.NoContent);
        }

        public async Task<Role> GuildCreateRole(Guild guild, SocketSendRole role, string reason = null)
        {
            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod(HttpMethod.Post).SetUri($"{Endpoint}/guilds/{guild.Id}/roles").SetJsonContent(EncodeJson(role));

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            HttpResponseMessage answer = await Send(message);

            return JsonConvert.DeserializeObject<Role>(await answer.Content.ReadAsStringAsync());
        }

        public async void GuildDeleteRole(Guild guild, Role role, string reason = null)
        {
            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/guilds/{guild.Id}/roles/{role.Id}");

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            await Send(message, HttpStatusCode.NoContent);
        }

        public async Task<SocketGuildChannel> GuildCreateChannel(Guild guild, SocketSendGuildChannel channel, string reason = null)
        {
            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod(HttpMethod.Post).SetUri($"{Endpoint}/guilds/{guild.Id}/channels").SetJsonContent(EncodeJson(channel));

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            HttpResponseMessage answer = await Send(message);
            return JsonConvert.DeserializeObject<SocketGuildChannel>(await answer.Content.ReadAsStringAsync());
        }

        public async Task<SocketGuildChannel> ChannelEdit(GuildChannel channel, SocketSendGuildChannel newChannel, string reason = null)
        {
            if ((int)channel.Type != newChannel.Type)
                throw new Exception("The two given channels are not the same type!\nYeah ik that you should be able to convert announcement channels to text ones and vice-versa but nuh uh");

            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod("PATCH").SetUri($"{Endpoint}/channels/{channel.Id}/channels").SetJsonContent(EncodeJson(channel));

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            HttpResponseMessage answer = await Send(message);
            return JsonConvert.DeserializeObject<SocketGuildChannel>(await answer.Content.ReadAsStringAsync());
        }

        public async void ChannelDelete(GuildChannel channel, string reason = null)
        {
            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/channels/{channel.Id}");

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            await Send(message);
        }

        public async void PinMessage(Message message, string reason = null)
        {
            HttpMessageBuilder request = HttpMessageBuilder.New().SetMethod(HttpMethod.Put).SetUri($"{Endpoint}/channels/{message.Channel.Id}/pins/{message.Id}");

            if (reason is not null)
                request.SetHeader("X-Audit-Log-Reason", reason);

            await Send(request, HttpStatusCode.NoContent);
        }

        public async void UnpinMessage(Message message, string reason = null)
        {
            HttpMessageBuilder request = HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/channels/{message.Channel.Id}/pins/{message.Id}");

            if (reason is not null)
                request.SetHeader("X-Audit-Log-Reason", reason);

            await Send(request, HttpStatusCode.NoContent);
        }

        public async Task<SocketGuildThreadChannel> StartThreadFromMessage(Message message, SocketSendPublicThread thread ,string reason = null)
        {
            HttpMessageBuilder request = HttpMessageBuilder.New().SetMethod(HttpMethod.Post).SetUri($"{Endpoint}/channels/{message.Channel.Id}/messages/{message.Id}/threads").SetJsonContent(EncodeJson(thread));

            if (reason is not null)
                request.SetHeader("X-Audit-Log-Reason", reason);

            HttpResponseMessage answer = await Send(request, HttpStatusCode.Created, true);

            string answ = await answer.Content.ReadAsStringAsync();

            Log.Warn($"Magic thing: {answer.StatusCode} - {answ}");

            return JsonConvert.DeserializeObject<SocketGuildThreadChannel>(await answer.Content.ReadAsStringAsync());
        }

        public async Task<SocketGuildThreadChannel> StartThread(GuildTextChannel channel, SocketSendPublicThread thread, string reason = null)
        {
            HttpMessageBuilder request = HttpMessageBuilder.New().SetMethod(HttpMethod.Post).SetUri($"{Endpoint}/channels/{channel.Id}/threads").SetJsonContent(EncodeJson(thread));

            if (reason is not null)
                request.SetHeader("X-Audit-Log-Reason", reason);

            HttpResponseMessage answer = await Send(request, HttpStatusCode.Created);

            return JsonConvert.DeserializeObject<SocketGuildThreadChannel>(await answer.Content.ReadAsStringAsync());
        }

        public async void JoinThread(GuildThreadChannel thread) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Put).SetUri($"{Endpoint}/channels/{thread.Id}/thread-members/@me"), HttpStatusCode.NoContent);

        public async void AddUserToThread(GuildThreadChannel thread, long id) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Put).SetUri($"{Endpoint}/channels/{thread.Id}/thread-members/{id}"), HttpStatusCode.NoContent);

        public async void LeaveThread(GuildThreadChannel thread) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/channels/{thread.Id}/thread-members/@me"), HttpStatusCode.NoContent);

        public async void RemoveUserToThread(GuildThreadChannel thread, long id) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/channels/{thread.Id}/thread-members/{id}"), HttpStatusCode.NoContent);

        public async Task<ThreadMember[]> GetThreadMembers(GuildThreadChannel thread)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Get).SetUri($"{Endpoint}/channels/{thread.Id}/thread-members"));
            return JsonConvert.DeserializeObject<ThreadMember[]>(await answer.Content.ReadAsStringAsync());
        }

        public async void AddOwnReaction(Message message, Emoji emoji) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Put).SetUri($"{Endpoint}/channels/{message.Channel.Id}/messages/{message.Id}/reactions/{emoji.Encode()}/@me"), HttpStatusCode.NoContent);

        public async void DeleteOwnReaction(Message message, Emoji emoji) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/channels/{message.Channel.Id}/messages/{message.Id}/reactions/{emoji.Encode()}/@me"), HttpStatusCode.NoContent);

        public async void DeleteUserReaction(Message message, Emoji emoji, long user) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/channels/{message.Channel.Id}/messages/{message.Id}/reactions/{emoji.Encode()}/{user}"), HttpStatusCode.NoContent);

        public async void DeleteAllReactions(Message message, Emoji emoji) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/channels/{message.Channel.Id}/messages/{message.Id}/reactions/{emoji.Encode()}"), HttpStatusCode.NoContent);

        public async void DeleteAllReactions(Message message) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/channels/{message.Channel.Id}/messages/{message.Id}/reactions"), HttpStatusCode.NoContent);

        public async Task<SocketMessage[]> GetMessages(GuildTextChannel channel, int? limit = 50, long? around = null, long? before = null, long? after = null)
        {
            string q = string.Empty;

            if (around is not null and not 0)
                q = $"&around={around}";

            if (before is not null and not 0)
                q = $"&before={before}";

            if (after is not null and not 0)
                q = $"&after={after}";

            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Get).SetUri($"{Endpoint}/channels/{channel.Id}/messages?limit={limit}{q}"));

            return JsonConvert.DeserializeObject<SocketMessage[]>(await answer.Content.ReadAsStringAsync());
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

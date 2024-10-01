using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SimpleDiscord.Components;
using SimpleDiscord.Components.Builders;
using SimpleDiscord.Components.DiscordComponents;
using SimpleDiscord.Enums;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDiscord.Networking
{
    internal class Http(HttpClient client, Client originalClient)
    {
        public HttpClient HttpClient { get; } = client;

        private readonly RateLimitHandler rateLimitHandler = new();

        private readonly Client discordClient = originalClient;

        public const string Endpoint = "https://discord.com/api/v10";

        public async Task<HttpResponseMessage> Send(HttpRequestMessage message, HttpStatusCode expectedResponse = HttpStatusCode.OK, bool disableResponseCheck = false, bool child = false)
        {
            if (!child)
                discordClient.Logger.Silent($"[HTTP BUCKET] {message.Method.Method} {message.RequestUri.OriginalString}");

            RateLimit rateLimit = rateLimitHandler.GetRateLimit(message);

            if (!rateLimit.Validate(out decimal waitingTime))
            {
                await Task.Delay(decimal.ToInt32(waitingTime) + 10);
                return await Send(message, expectedResponse, disableResponseCheck, true);
            }

            rateLimitHandler.MakeRequest(message); // Sync update

            // Check RL also here!
            if (rateLimit.Remaining < -1)
            {
                await Task.Delay(500);
                return await Send(message, expectedResponse, disableResponseCheck, true);
            }

            //discordClient.Logger.Info($"Sending HTTP content:\n{await message.Content.ReadAsStringAsync()}");
            HttpResponseMessage answer = await HttpClient.SendAsync(message);

            rateLimitHandler.UpdateRateLimit(message, answer.Headers); //  Async update

            if (!disableResponseCheck && answer.StatusCode != expectedResponse)
                discordClient.ErrorHub.Throw($"Failed to send a HTTP {message.Method.Method} request to {message.RequestUri.OriginalString}.\nExpected OK (200), got {answer.StatusCode} ({(int)answer.StatusCode})");

            return answer;
        }

        public async Task<SocketMessage> SendMessage(GuildTextChannel channel, SocketSendMessage message)
        {
            if (message.Components is not null)
                foreach (SocketActionRow actionRow in message.Components)
                    foreach (object button in actionRow.Components)
                        if (button is not null && button is Button buttonComponent)
                            if (buttonComponent.Callback is not null)
                                discordClient._discordClient.buttonCallbacks.Add(buttonComponent.CustomId, new(buttonComponent.Data, buttonComponent.Callback));

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
                discordClient.ErrorHub.Throw("Limit cannot be greater than 100!", true);

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
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod("PATCH").SetUri($"{Endpoint}/webhooks/{interaction.ApplicationId}/{interaction.Token}/messages/@original").SetJsonContent(EncodeJson(response.ToSocketInstance().Data)));

            SocketInteractionResponse socketInteraction = JsonConvert.DeserializeObject<SocketInteractionResponse>(await answer.Content.ReadAsStringAsync());
            interaction.SafeUpdateResponse(socketInteraction);
            return new(socketInteraction);
        }

        public async Task DeleteInteractionReply(Interaction interaction)
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
                discordClient.ErrorHub.Throw($"Failed to send a HTTP request!.\nExpected OK (200), got {answer.StatusCode} ({(int)answer.StatusCode})");

            return JsonConvert.DeserializeObject<SocketApplicationCommand>(await answer.Content.ReadAsStringAsync());
        }

        public async Task<SocketApplicationCommand> EditGlobalCommand(ApplicationCommand command, SocketSendApplicationCommand newCommand)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod("PATCH").SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/commands/{command.Id}").SetJsonContent(EncodeJson(newCommand)));

            return JsonConvert.DeserializeObject<SocketApplicationCommand>(await answer.Content.ReadAsStringAsync());
        }

        public async  Task DeleteGlobalCommand(ApplicationCommand command) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/commands/{command.Id}"), HttpStatusCode.NoContent);

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
                discordClient.ErrorHub.Throw($"Failed to send a HTTP request!.\nExpected OK (200), got {answer.StatusCode} ({(int)answer.StatusCode})");

            return JsonConvert.DeserializeObject<SocketApplicationCommand>(await answer.Content.ReadAsStringAsync());
        }

        public async Task<SocketApplicationCommand> EditGuildCommand(Guild guild, ApplicationCommand command, SocketSendApplicationCommand newCommand)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod("PATCH").SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/guilds/{guild.Id}/commands/{command.Id}").SetJsonContent(EncodeJson(newCommand)));

            return JsonConvert.DeserializeObject<SocketApplicationCommand>(await answer.Content.ReadAsStringAsync());
        }

        public async  Task DeleteGuildCommand(Guild guild, ApplicationCommand command) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/guilds/{guild.Id}/commands/{command.Id}"), HttpStatusCode.NoContent);

        public async Task<SocketApplicationCommand[]> BulkOverwriteGuildCommands(Guild guild, SocketApplicationCommand[] commands)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Put).SetUri($"{Endpoint}/applications/{discordClient.Application.Id}/guilds/{guild.Id}/commands").SetJsonContent(EncodeJson(commands)));

            return JsonConvert.DeserializeObject<SocketApplicationCommand[]>(await answer.Content.ReadAsStringAsync());
        }

        public Task MemberAddRole(Member member, Role role, string reason = null) => MemberAddRole(member, role.Id, reason);

        public async Task MemberAddRole(Member member, long role, string reason = null)
        {
            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod(HttpMethod.Put).SetUri($"{Endpoint}/guilds/{member.Guild.Id}/members/{member.User.Id}/roles/{role}");

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            await Send(message, HttpStatusCode.NoContent);
        }

        public Task MemberRemoveRole(Member member, Role role, string reason = null) => MemberRemoveRole(member, role.Id, reason);

        public async Task MemberRemoveRole(Member member, long role, string reason = null)
        {
            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/guilds/{member.Guild.Id}/members/{member.User.Id}/roles/{role}");

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            await Send(message, HttpStatusCode.NoContent);
        }

        public async Task MemberKick(Member member, string reason = null)
        {
            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/guilds/{member.Guild.Id}/members/{member.User.Id}");

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            await Send(message, HttpStatusCode.NoContent);
        }

        public async Task MemberBan(Member member, string reason = null, int deleteMessageSeconds = 0)
        {
            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod(HttpMethod.Put).SetUri($"{Endpoint}/guilds/{member.Guild.Id}/bans/{member.User.Id}?delete_message_seconds={deleteMessageSeconds}");

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            await Send(message, HttpStatusCode.NoContent);
        }

        public async Task MemberUnban(Guild guild, long userId, string reason = null)
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

        public async Task GuildDeleteRole(Guild guild, Role role, string reason = null)
        {
            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/guilds/{guild.Id}/roles/{role.Id}");

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            await Send(message, HttpStatusCode.NoContent);
        }

        public async Task<GuildChannel> GuildCreateChannel(Guild guild, SocketSendGuildChannel channel, string reason = null)
        {
            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod(HttpMethod.Post).SetUri($"{Endpoint}/guilds/{guild.Id}/channels").SetJsonContent(EncodeJson(channel));

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            HttpResponseMessage answer = await Send(message);
            return new(JsonConvert.DeserializeObject<SocketGuildChannel>(await answer.Content.ReadAsStringAsync()), false);
        }

        public async Task<GuildChannel> ChannelEdit(GuildChannel channel, SocketSendGuildChannel newChannel, string reason = null)
        {
            if ((int)channel.Type != newChannel.Type)
                discordClient.ErrorHub.Throw("The two given channels are not the same type!\nYeah ik that you should be able to convert announcement channels to text ones and vice-versa but nuh uh", true);

            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod("PATCH").SetUri($"{Endpoint}/channels/{channel.Id}/channels").SetJsonContent(EncodeJson(channel));

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            HttpResponseMessage answer = await Send(message);
            return new(JsonConvert.DeserializeObject<SocketGuildChannel>(await answer.Content.ReadAsStringAsync()), false);
        }

        public async Task ChannelDelete(GuildChannel channel, string reason = null)
        {
            HttpMessageBuilder message = HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/channels/{channel.Id}");

            if (reason is not null)
                message.SetHeader("X-Audit-Log-Reason", reason);

            await Send(message);
        }

        public async Task PinMessage(Message message, string reason = null)
        {
            HttpMessageBuilder request = HttpMessageBuilder.New().SetMethod(HttpMethod.Put).SetUri($"{Endpoint}/channels/{message.Channel.Id}/pins/{message.Id}");

            if (reason is not null)
                request.SetHeader("X-Audit-Log-Reason", reason);

            await Send(request, HttpStatusCode.NoContent);
        }

        public async Task UnpinMessage(Message message, string reason = null)
        {
            HttpMessageBuilder request = HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/channels/{message.Channel.Id}/pins/{message.Id}");

            if (reason is not null)
                request.SetHeader("X-Audit-Log-Reason", reason);

            await Send(request, HttpStatusCode.NoContent);
        }

        public async Task<GuildThreadChannel> StartThreadFromMessage(Message message, SocketSendPublicThread thread ,string reason = null)
        {
            HttpMessageBuilder request = HttpMessageBuilder.New().SetMethod(HttpMethod.Post).SetUri($"{Endpoint}/channels/{message.Channel.Id}/messages/{message.Id}/threads").SetJsonContent(EncodeJson(thread));

            if (reason is not null)
                request.SetHeader("X-Audit-Log-Reason", reason);

            HttpResponseMessage answer = await Send(request, HttpStatusCode.Created, true);

            return new(JsonConvert.DeserializeObject<SocketGuildThreadChannel>(await answer.Content.ReadAsStringAsync()), false);
        }

        public async Task<GuildThreadChannel> StartThread(GuildTextChannel channel, SocketSendPublicThread thread, string reason = null)
        {
            HttpMessageBuilder request = HttpMessageBuilder.New().SetMethod(HttpMethod.Post).SetUri($"{Endpoint}/channels/{channel.Id}/threads").SetJsonContent(EncodeJson(thread));

            if (reason is not null)
                request.SetHeader("X-Audit-Log-Reason", reason);

            HttpResponseMessage answer = await Send(request, HttpStatusCode.Created);

            return new(JsonConvert.DeserializeObject<SocketGuildThreadChannel>(await answer.Content.ReadAsStringAsync()), false);
        }

        public async Task JoinThread(GuildThreadChannel thread) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Put).SetUri($"{Endpoint}/channels/{thread.Id}/thread-members/@me"), HttpStatusCode.NoContent);

        public async Task AddUserToThread(GuildThreadChannel thread, long id) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Put).SetUri($"{Endpoint}/channels/{thread.Id}/thread-members/{id}"), HttpStatusCode.NoContent);

        public async Task LeaveThread(GuildThreadChannel thread) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/channels/{thread.Id}/thread-members/@me"), HttpStatusCode.NoContent);

        public async Task RemoveUserToThread(GuildThreadChannel thread, long id) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/channels/{thread.Id}/thread-members/{id}"), HttpStatusCode.NoContent);

        public async Task<ThreadMember[]> GetThreadMembers(GuildThreadChannel thread)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Get).SetUri($"{Endpoint}/channels/{thread.Id}/thread-members"));
            return JsonConvert.DeserializeObject<ThreadMember[]>(await answer.Content.ReadAsStringAsync());
        }

        public async Task AddOwnReaction(Message message, Emoji emoji) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Put).SetUri($"{Endpoint}/channels/{message.Channel.Id}/messages/{message.Id}/reactions/{emoji.Encode()}/@me"), HttpStatusCode.NoContent);

        public async Task DeleteOwnReaction(Message message, Emoji emoji) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/channels/{message.Channel.Id}/messages/{message.Id}/reactions/{emoji.Encode()}/@me"), HttpStatusCode.NoContent);

        public async Task DeleteUserReaction(Message message, Emoji emoji, long user) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/channels/{message.Channel.Id}/messages/{message.Id}/reactions/{emoji.Encode()}/{user}"), HttpStatusCode.NoContent);

        public async Task DeleteAllReactions(Message message, Emoji emoji) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/channels/{message.Channel.Id}/messages/{message.Id}/reactions/{emoji.Encode()}"), HttpStatusCode.NoContent);

        public async Task DeleteAllReactions(Message message) => await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Delete).SetUri($"{Endpoint}/channels/{message.Channel.Id}/messages/{message.Id}/reactions"), HttpStatusCode.NoContent);

        public async Task<Message[]> GetMessages(GuildTextChannel channel, int? limit = 50, long? around = null, long? before = null, long? after = null)
        {
            string q = string.Empty;

            if (around is not null and not 0)
                q = $"&around={around}";

            if (before is not null and not 0)
                q = $"&before={before}";

            if (after is not null and not 0)
                q = $"&after={after}";

            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Get).SetUri($"{Endpoint}/channels/{channel.Id}/messages?limit={limit}{q}"));

            List<Message> messages = [];

            foreach (SocketMessage message in JsonConvert.DeserializeObject<SocketMessage[]>(await answer.Content.ReadAsStringAsync()))
                messages.Add(new(message, channel));

            return [.. messages];
        }
        public async Task<Message> EndPoll(Poll poll)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Post).SetUri($"{Endpoint}/channels/{poll.Message.Channel.Id}/polls/{poll.Message.Id}/expire"));
            SocketMessage message = JsonConvert.DeserializeObject<SocketMessage>(await answer.Content.ReadAsStringAsync());
            discordClient._discordClient.pollResults.Add(poll.Message.Id);
            return new(message);
        }

        public async Task<Message> GetMessage(GuildTextChannel channel, long id)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Get).SetUri($"{Endpoint}/channels/{channel.Id}/messages/{id}"));
            SocketMessage message = JsonConvert.DeserializeObject<SocketMessage>(await answer.Content.ReadAsStringAsync());
            return new(message);
        }

        public async Task<GuildThreadChannel[]> GetGuildThreads(Guild guild)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Get).SetUri($"{Endpoint}/guilds/{guild.Id}/threads/active"));
            SocketThreadList message = JsonConvert.DeserializeObject<SocketThreadList>(await answer.Content.ReadAsStringAsync());

            List<GuildThreadChannel> threads = [];

            foreach (SocketGuildThreadChannel thread in message.Threads)
                threads.Add(new(thread));

            return [..threads];
        }

        public async Task<GuildThreadChannel[]> GetChannelArchivedThreads(GuildTextChannel channel, bool @public = true, int? limit = null)
        {
            string uri = $"{Endpoint}/channels/{channel.Id}/threads/archived/{(@public ? "public" : "private")}";

            if (limit is not null)
                uri += $"?limit={limit}";

            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Get).SetUri(uri));
            SocketThreadList message = JsonConvert.DeserializeObject<SocketThreadList>(await answer.Content.ReadAsStringAsync());

            List<GuildThreadChannel> threads = [];

            foreach (SocketGuildThreadChannel thread in message.Threads)
                threads.Add(new(thread));

            return [.. threads];
        }

        public async Task<VoiceState> GetMemberVoiceState(Member member)
        {
            string uri = $"{Endpoint}/guilds/{member.Guild.Id}/voice-states/";
            if (member.User.Id == discordClient.CurrentUser.Id)
                uri += "@me";
            else
                uri += member.User.Id.ToString();

            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Get).SetUri(uri));
            return new(JsonConvert.DeserializeObject<SocketVoiceState>(await answer.Content.ReadAsStringAsync()));
        }

        public async Task<Member> GetGuildMember(Guild guild, long userId)
        {
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod(HttpMethod.Get).SetUri($"{Endpoint}/guilds/{guild.Id}/members/{userId}"));
            return new(guild, JsonConvert.DeserializeObject<SocketMember>(await answer.Content.ReadAsStringAsync()));
        }

        public async Task<Member> ModifyGuildMember(Guild guild, Member member, object update)
        {
            discordClient.Logger.Info(EncodeJson(update));
            HttpResponseMessage answer = await Send(HttpMessageBuilder.New().SetMethod("PATCH").SetUri($"{Endpoint}/guilds/{guild.Id}/members/{member.User.Id}").SetContent(new StringContent(EncodeJson(update), Encoding.UTF8, "application/json")));
            return new(guild, JsonConvert.DeserializeObject<SocketMember>(await answer.Content.ReadAsStringAsync()));
        }

        internal static T Sync<T>(Task<T> task)
        {
            task.Wait();
            return task.Result;
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

﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleDiscord.Components.Attributes;
using SimpleDiscord.Components.DiscordComponents;
using SimpleDiscord.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleDiscord.Components
{
#nullable enable
    [SocketInstance(typeof(SocketMessage))]
    public class Message : SocketMessage
    {
        public GuildTextChannel Channel { get; }

        public Member Member { get; }

        public Guild Guild { get; }

        public new GuildThreadChannel? Thread { get; }

        public new List<Reaction>? Reactions { get; private set; }

        public new List<JObject>? Components { get; }

        public new Poll? Poll { get; }

        public string Link { get; }

        public Message(SocketMessage baseMessage, GuildTextChannel? channel = null) : base(baseMessage)
        {
            channel ??= GuildChannel.List.FirstOrDefault(channel => channel.Id == ChannelId) as GuildTextChannel;

#nullable disable
            Channel = channel;

            Thread = null;
            if (baseMessage.Thread is not null)
                Thread = new(baseMessage.Thread);

            Guild = Channel.Guild;
            Member = Guild.GetMember(baseMessage.Author.Id);

            Client ??= Guild.Client;

            Components = null;
            if (baseMessage.Components is not null && baseMessage.Components.Length > 0)
            {
                Components = [];
                foreach (object socketActionRowObject in baseMessage.Components)
                    if (socketActionRowObject is JObject obj)
                        Components.Add(obj);
            }

            if (baseMessage.Poll is not null)
                Poll = new(this, baseMessage.Poll);

            List<Reaction> _reactions = [];
            if (baseMessage.Reactions is not null)
                foreach (SocketPartialReaction socketPartialReaction in baseMessage.Reactions)
                    _reactions.Add(new(this, socketPartialReaction));

            Reactions = [.. _reactions];

            Link = $"https://discord.com/channels/{GuildId}/{ChannelId}/{Id}";

            Channel.SafeUpdateMessage(this);
        }

        internal void SafeUpdateReaction(Reaction reaction)
        {
            Reactions ??= [];

            Reaction instance = Reactions.FirstOrDefault(r => r.Emoji.Encode() == reaction.Emoji.Encode());
            if (instance is null)
                Reactions.Add(reaction);
            else
                Reactions[Reactions.IndexOf(instance)] = reaction;
        }

        public Task<SocketMessage> Edit(SocketSendMessage message) => Client.RestHttp.EditMessage(this, message);

        public Task<SocketMessage> Edit(string content, List<Embed> embeds = null) => Edit(new(content, embeds, null, null, null, null, null));

        public Task<SocketMessage> Reply(string content, List<Embed> embeds = null) => Channel.SendMessage(new SocketSendMessage(content, embeds, null, new MessageReference(0, Id, ChannelId, GuildId), null, null, null));

        public Task<bool> Delete(string reason = null) => Client.RestHttp.DeleteMessage(this, reason);

        public Task Pin(string reason = null) => Client.RestHttp.PinMessage(this, reason);

        public Task Unpin(string reason = null) => Client.RestHttp.UnpinMessage(this, reason);

#nullable enable
        public Task<GuildThreadChannel>? StartThread(SocketSendPublicThread threadConfig, string? reason = null) => Channel is not GuildThreadChannel ? Client.RestHttp.StartThreadFromMessage(this, threadConfig, reason) : null;
#nullable disable

        public Task React(Emoji emoji) => Client.RestHttp.AddOwnReaction(this, emoji);

        public Task Unreact(Emoji emoji) => Client.RestHttp.DeleteOwnReaction(this, emoji);

        public Task RemoveUserReaction(Emoji emoji, long user) => Client.RestHttp.DeleteUserReaction(this, emoji, user);

#nullable enable
        public Task RemoveEveryReaction(Emoji? emoji = null)
        {
            if (emoji is null) // Remove all
                return Client.RestHttp.DeleteAllReactions(this);
            else
                return Client.RestHttp.DeleteAllReactions(this, emoji);
        }

        public static string[] Chunk(string fullMessage)
        {
            List<string> chunks = [string.Empty];
            int index = 0;

            foreach (string part in fullMessage.Split(' '))
                if (chunks[index].Length + part.Length + 1 > 1999)
                {
                    index++;
                    chunks.Add(part);
                }
                else
                    chunks[index] += $"{part} ";

            return [.. chunks];
        }

        public override bool Equals(object obj) => obj is Message message ? Id == message.Id : base.Equals(obj);

        public override int GetHashCode() => base.GetHashCode();
    }
}

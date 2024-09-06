using SimpleDiscord.Components.Attributes;
using SimpleDiscord.Components.DiscordComponents;
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

        public SocketMember MemberAuthor { get; }

        public Guild Guild { get; }

        public new GuildThreadChannel? Thread { get; }

        public new List<Reaction>? Reactions { get; }

        public new List<ActionRow>? Components { get; }

        public Message(SocketMessage baseMessage) : base(baseMessage)
        {
            Console.WriteLine($"Called message ..ctor with {baseMessage.Content} - {baseMessage.Id}\n\n");

            Thread = null;
            if (baseMessage.Thread is not null)
                Thread = new(baseMessage.Thread);
#nullable disable
            Console.WriteLine($"We have a total of {GuildChannel.List.Count} cached channels!");
            Channel = GuildChannel.List.FirstOrDefault(channel => channel.Id == ChannelId) as GuildTextChannel;

            Guild = Channel.Guild;
            MemberAuthor = Guild.GetMember(baseMessage.Author.Id);

            Components = null;
            if (baseMessage.Components is not null && baseMessage.Components.Length > 0)
            {
                Components = [];
                foreach (SocketActionRow socketActionRow in baseMessage.Components)
                    Components.Add(new(socketActionRow));
            }

            List<Reaction> _reactions = [];
            if (baseMessage.Reactions is not null)
                foreach (SocketPartialReaction socketPartialReaction in baseMessage.Reactions)
                    _reactions.Add(new(this, socketPartialReaction));

            Reactions = [.. _reactions];

            Channel.SafeUpdateMessage(this);
        }

        internal void SafeUpdateReaction(Reaction reaction)
        {
            Reaction instance = Reactions.FirstOrDefault(r => r.Emoji.Encode() == reaction.Emoji.Encode());
            if (instance is null)
                Reactions.Add(reaction);
            else
                Reactions[Reactions.IndexOf(instance)] = reaction;
        }

        public Task<SocketMessage> Edit(SocketSendMessage message) => Client.RestHttp.EditMessage(this, message);

        public Task<SocketMessage> Edit(string content, Embed[] embeds = null) => Edit(new(content, embeds, null, null, null, null, null));

        public Task<SocketMessage> Reply(string content, Embed[] embeds = null) => Channel.SendMessage(new SocketSendMessage(content, embeds, null, new MessageReference(0, Id, ChannelId, GuildId), null, null, null));

        public Task<bool> Delete(string reason = null) => Client.RestHttp.DeleteMessage(this, reason);

        public void Pin(string reason = null) => Client.RestHttp.PinMessage(this, reason);

        public void Unpin(string reason = null) => Client.RestHttp.UnpinMessage(this, reason);

#nullable enable
        public Task<SocketGuildThreadChannel>? StartThread(SocketSendPublicThread threadConfig, string? reason = null) => Channel is not GuildThreadChannel ? Client.RestHttp.StartThreadFromMessage(this, threadConfig, reason) : null;
#nullable disable

        public void React(Emoji emoji) => Client.RestHttp.AddOwnReaction(this, emoji);

        public void Unreact(Emoji emoji) => Client.RestHttp.DeleteOwnReaction(this, emoji);

        public void RemoveUserReaction(Emoji emoji, long user) => Client.RestHttp.DeleteUserReaction(this, emoji, user);

#nullable enable
        public void RemoveEveryReaction(Emoji? emoji = null)
        {
            if (emoji is null) // Remove all
                Client.RestHttp.DeleteAllReactions(this);
            else
                Client.RestHttp.DeleteAllReactions(this, emoji);
        }
    }
}

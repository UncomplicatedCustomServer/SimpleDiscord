using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleDiscord.Components.DiscordComponents;
using SimpleDiscord.Enums;
using SimpleDiscord.Logger;
using System;
using System.Threading.Tasks;

namespace SimpleDiscord.Components
{
#nullable enable
    public class Interaction : SocketInteraction
    {
        public Guild Guild { get; }

        public new InteractionType Type { get; }

        public GuildChannel Channel { get; }

        public new Message? Message { get; }

        public new InteractionData? Data { get; internal set; }

        public InteractionResponse? Response { get; private set; } = null;

        [JsonIgnore]
        public bool OnHold { get; private set; }

        public bool HasData => Type is not InteractionType.PING;

        public Interaction(SocketInteraction socketInteraction) : base(socketInteraction)
        {
            if (socketInteraction.GuildId is null)
                throw new NullReferenceException("GuildId at SocketInteraction::GuildId is null!");

            if (socketInteraction.ChannelId is null)
                throw new NullReferenceException("GuildId at SocketInteraction::ChannelId is null!");

            Type = (InteractionType)socketInteraction.Type;

            Guild = Guild.GetSafeGuild((long)socketInteraction.GuildId);
            Channel = Guild.GetSafeChannel((long)socketInteraction.ChannelId);

            Message = null;
            if (socketInteraction.Message is not null)
                Message = new(socketInteraction.Message);

            Data = null;
            if (socketInteraction.Data is not null && socketInteraction.Data is JObject obj)
                Data = InteractionData.Caster(this, obj);
        }

        internal void SafeUpdateResponse(SocketInteractionResponse response) => Response = new(response);

        internal void ClearResponse() => Response = null;

        /// <summary>
        /// Acknowledge the interaction without a message and/or a loading state.<br></br>
        /// Only valid for message components and modals.
        /// </summary>
        public Task<InteractionResponse> Acknowledge() => AcknowledgeWithoutLoading();

        /// <summary>
        /// Acknowledge the interaction with an instant message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ephemeral"></param>
        /// <returns></returns>
        public Task<InteractionResponse> AcknowledgeWithMessage(SocketSendMessage message, bool ephemeral = true)
        {
            if (ephemeral)
                message.Flags = (int)MessageFlags.EPHEMERAL;

            return Client.RestHttp.SendInteractionReply(this, new(InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE, message));
        }

        /// <summary>
        /// Acknowledge the interaction with a loading state, visible to the user.
        /// </summary>
        /// <returns></returns>
        public Task<InteractionResponse> AcknowledgeWithLoading(bool ephemeral = true)
        {
            SocketSendMessage message = new();
            if (ephemeral)
                message.Flags = (int)MessageFlags.EPHEMERAL;

            return Client.RestHttp.SendInteractionReply(this, new(InteractionCallbackType.DEFERRED_CHANNEL_MESSAGE_WITH_SOURCE, message));
        }

        /// <summary>
        /// Acknowledge the interaction without a message and/or a loading state.<br></br>
        /// Only valid for message components and modals.
        /// </summary>
        /// <returns></returns>
        public Task<InteractionResponse> AcknowledgeWithoutLoading() => Client.RestHttp.SendInteractionReply(this, new(InteractionCallbackType.DEFERRED_UPDATE_MESSAGE, null));

        /// <summary>
        /// Update the original response to the interaction.<br></br>
        /// You have to use it also to give an answer to a loading state created with <see cref="AcknowledgeWithLoading(bool)"/>.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ephemeral"></param>
        /// <returns></returns>
        public Task<InteractionResponse> UpdateOriginalMessage(SocketSendMessage message, bool ephemeral = true)
        {
            if (ephemeral)
                message.Flags = (int)MessageFlags.EPHEMERAL;

            return Client.RestHttp.EditInteractionReply(this, new(InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE, message));
        }

        /// <summary>
        /// Delete the interaction original response.
        /// </summary>
        public Task DeleteOriginalMessage() => Client.RestHttp.DeleteInteractionReply(this);

        /// <summary>
        /// Acknowledge the interaction by opening a modal.<br></br>
        /// Can't be used if this interaction is a modal one.
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        public Task<InteractionResponse> AcknowledgeByOpeningModal(Modal modal) => Client.RestHttp.SendInteractionReply(this, new(InteractionCallbackType.MODAL, modal));

    }
}
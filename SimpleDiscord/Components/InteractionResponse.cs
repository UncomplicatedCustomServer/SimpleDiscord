using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleDiscord.Enums;
using System;

namespace SimpleDiscord.Components.DiscordComponents
{
#nullable enable
    public class InteractionResponse : SocketInteractionResponse
    {
        public new InteractionCallbackType Type { get; }
        public new InteractionCallbackData? Data { get; }

        [JsonConstructor]
        public InteractionResponse(int type, InteractionCallbackData? data) : base(type, data)
        {
            Type = (InteractionCallbackType)type;
            Data = data;
        }

        public InteractionResponse(InteractionCallbackType type, InteractionCallbackData? data) : this((int)type, data)
        {
            Type? requiredType = Typize(type);
            if (requiredType is null && data is not null)
                throw new Exception("Data is required to be null!");
            else if (requiredType is not null)
                if (requiredType != data?.GetType())
                    throw new Exception($"The given type ({data?.GetType().Name}) was not expected!\nExpected {requiredType} for type {type}");
        }

        public InteractionResponse(SocketInteractionResponse socketInstance) : base(socketInstance)
        {
            Type = (InteractionCallbackType)socketInstance.Type;
            Type? requiredType = Typize(Type);

            if (requiredType is null)
                Data = null;
            else if (requiredType is not null && socketInstance.Data is JObject obj)
                Data = obj.ToObject(requiredType) as InteractionCallbackData;
        }

        public SocketInteractionResponse ToSocketInstance() => new((int)Type, Data);

        public static Type? Typize(InteractionCallbackType type)
        {
            if (type is InteractionCallbackType.APPLICATION_COMMAND_AUTOCOMPLETE_RESULT or InteractionCallbackType.PREMIUM_REQUIRED or InteractionCallbackType.LAUNCH_ACTIVITY)
                throw new NotImplementedException($"Interaction type {type} is not supported yet!");

            return type switch
            {
                InteractionCallbackType.PONG => null,
                InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE => typeof(SocketSendMessage),
                InteractionCallbackType.DEFERRED_CHANNEL_MESSAGE_WITH_SOURCE or InteractionCallbackType.DEFERRED_UPDATE_MESSAGE => typeof(SocketSendMessage),
                InteractionCallbackType.UPDATE_MESSAGE => typeof(SocketSendMessage),
                InteractionCallbackType.MODAL => typeof(Modal),
                _ => null,
            };
        }
    }
}

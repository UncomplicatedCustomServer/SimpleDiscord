using System;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
#nullable enable
    internal class EventIntents(Type type, int opCode = 0, string? @event = null)
    {
        public string? Event { get; } = @event;

        public int OpCode { get; } = opCode;

        public Type Type { get; } = type;
    }
}

using System;

namespace SimpleDiscord.Gateway.Events.Attributes
{
#nullable enable
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class InternalEvent : Attribute
    {
        public string? Event { get; }

        public int OpCode { get; }

        public InternalEvent(int opCode)
        {
            Event = null;
            OpCode = opCode;
        }

        public InternalEvent(string @event)
        {
            Event = @event;
            OpCode = 0;
        }
    }
}

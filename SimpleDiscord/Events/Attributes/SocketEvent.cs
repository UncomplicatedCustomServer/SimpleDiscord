using System;

namespace SimpleDiscord.Events.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SocketEvent(string name) : Attribute
    {
        public string Event { get; } = name;
    }
}

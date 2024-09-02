using System;

namespace SimpleDiscord.Events.Attribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SocketEvent(string name) : System.Attribute
    {
        public string Event { get; } = name;
    }
}

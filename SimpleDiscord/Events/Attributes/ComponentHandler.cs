using System;

namespace SimpleDiscord.Events.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ComponentHandler(string componentId, bool fullMatch = true) : Attribute
    {
        public string ComponentId { get; } = componentId;

        public bool FullMatch { get; } = fullMatch;
    }
}

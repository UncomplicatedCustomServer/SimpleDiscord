using System;

namespace SimpleDiscord.Events.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ComponentHandler(string componentId) : Attribute
    {
        public string ComponentId { get; } = componentId;
    }
}

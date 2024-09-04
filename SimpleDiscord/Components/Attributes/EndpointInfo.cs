using System;

namespace SimpleDiscord.Components.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    internal class EndpointInfo(string endpoint, string propertyName) : Attribute
    {
        public string Endpoint { get; } = endpoint;

        public string PropertyName { get; } = propertyName;
    }
}

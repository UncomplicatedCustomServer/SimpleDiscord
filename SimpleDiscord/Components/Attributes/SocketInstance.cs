using System;

namespace SimpleDiscord.Components.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    internal class SocketInstance(Type instance) : Attribute
    {
        public Type Instance { get; } = instance;
    }
}

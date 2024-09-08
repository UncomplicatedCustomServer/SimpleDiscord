using System;

namespace SimpleDiscord.Events.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandHandler(string commandName) : Attribute
    {
        public string CommandName { get; } = commandName;
    }
}

using System;

namespace SimpleDiscord.Events.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandHandler(string commandName, string subCommandName = null) : Attribute
    {
        public string CommandName { get; } = commandName;

        public string SubCommandName { get; } = subCommandName;
    }
}

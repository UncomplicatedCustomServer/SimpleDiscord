using SimpleDiscord.Enums;

namespace SimpleDiscord
{
    public sealed class ClientConfig
    {
        public bool SaveMessages { get; set; } = true;
        
        public RegisterCommandType RegisterCommands { get; set; } = RegisterCommandType.None; // This will override Load<both global and guild>RegisteredCommands to false and will override every registered command - by default we don't allow command registering

        public bool SaveGlobalRegisteredCommands { get; set; } = true;

        public bool SaveGuildRegisteredCommands { get; set; } = true;

        public bool LoadAppInfo { get; set; } = false; // If false the Application prop in Client will be partial
    }
}

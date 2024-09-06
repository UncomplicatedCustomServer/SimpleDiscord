using SimpleDiscord.Enums;

namespace SimpleDiscord
{
    public sealed class ClientConfig
    {
        public bool Debug { get; set; } = false;

        public bool LoadMembers { get; set; } = true; // That requires lots of ram :)

        public bool SaveMessages { get; set; } = true;

        public bool FetchThreadMembers { get; set; } = true;
        
        public RegisterCommandType RegisterCommands { get; set; } = RegisterCommandType.None;

        public bool SaveGlobalRegisteredCommands { get; set; } = true;

        public bool SaveGuildRegisteredCommands { get; set; } = true;

        public bool LoadAppInfo { get; set; } = false; // If false the Application prop in Client will be partial
    }
}

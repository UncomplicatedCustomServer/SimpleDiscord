using System.Collections.Generic;

namespace SimpleDiscord.Components.Helpers
{
    public static class MemberEditor
    {
        public static Dictionary<string, string> ChangeNickname(string nick) => new()
        {
            {
                "nick",
                nick
            }
        };

        public static Dictionary<string, bool> SetMute(bool muted) => new()
        {
            {
                "mute",
                muted
            }
        };

        public static Dictionary<string, bool> SetDeaf(bool deaf) => new()
        {
            {
                "deaf",
                deaf
            }
        };

#nullable enable
        public static Dictionary<string, long?> Move(long? channelId) => new()
        {
            {
                "channel_id",
                channelId
            }
        };
    }
}

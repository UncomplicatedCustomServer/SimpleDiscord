using SimpleDiscord.Components.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleDiscord.Components
{
#nullable enable
    [SocketInstance(typeof(SocketPartialReaction))]
    public class Reaction : SocketPartialReaction
    {
        public Message Message { get; }

        public List<SocketUser> Members { get; }

        public List<SocketUser> BurstMembers { get; }

        public int TotalCount => CountDetails is not null ? CountDetails["burst"] + CountDetails["normal"] : -1;

        public Reaction(Message message, SocketPartialReaction socketPartialReaction, bool fetchMembers = false) : base(socketPartialReaction)
        {
            Message = message;
            Members = [];
            BurstMembers = [];

            if (socketPartialReaction.Emoji is null)
                Message.Client.ErrorHub.Throw("Emoji is null", true);

            if (fetchMembers && CountDetails is not null && CountDetails.TryGetValue("burst", out int burstCount) && CountDetails.TryGetValue("normal", out int normalCount))
            {
                if (Members.Count < normalCount)
                {
                    Task<SocketUser[]> normalMembers = Message.Guild.Client.RestHttp.GetReactions(message, Emoji.Encode(), Enums.ReactionType.NORMAL, 100);
                    normalMembers.Wait();
                    Members = [.. normalMembers.Result];
                }

                if (BurstMembers.Count < burstCount)
                {
                    Task<SocketUser[]> burstMembers = Message.Guild.Client.RestHttp.GetReactions(message, Emoji.Encode(), Enums.ReactionType.BURST, 100);
                    burstMembers.Wait();
                    BurstMembers = [.. burstMembers.Result];
                }
            }

            Message.SafeUpdateReaction(this);
        }
    }
}

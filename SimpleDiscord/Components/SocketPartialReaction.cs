using Newtonsoft.Json;
using System.Collections.Generic;

namespace SimpleDiscord.Components
{
    public class SocketPartialReaction
    {
        public int Count { get; }

        [JsonProperty("count_details")]
        public Dictionary<string, int> CountDetails { get; }

        public bool Me { get; }

        [JsonProperty("me_burst")]
        public bool MeBurst { get; }

        public Emoji Emoji { get; }

        [JsonProperty("burst_colors")]
        public int[] BurstColors { get; }

        [JsonConstructor]
        public SocketPartialReaction(int count, Dictionary<string, int> countDetails, bool me, bool meBurst, Emoji emoji, int[] burstColors)
        {
            Count = count;
            CountDetails = countDetails;
            Me = me;
            MeBurst = meBurst;
            Emoji = emoji;
            BurstColors = burstColors;
        }

        public SocketPartialReaction(SocketPartialReaction self)
        {
            Count = self.Count;
            CountDetails = self.CountDetails;
            Me = self.Me;
            MeBurst = self.MeBurst;
            Emoji = self.Emoji;
            BurstColors = self.BurstColors;
        }
    }
}

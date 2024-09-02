namespace SimpleDiscord.Components
{
    public class Reaction(int count, object countDetails, bool me, bool meBurst, string emoji, int[] burstColors)
    {
        public int Count { get; } = count;

        public object CountDetails { get; } = countDetails;

        public bool Me { get; } = me;

        public bool MeBurst { get; } = meBurst;

        public string Emoji { get; } = emoji;

        public int[] BurstColors { get; } = burstColors;
    }
}

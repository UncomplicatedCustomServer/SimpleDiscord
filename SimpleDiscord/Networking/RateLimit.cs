using System;

namespace SimpleDiscord.Networking
{
    internal class RateLimit(int initialLimit, int initialRemain, float resetAfter, float reset)
    {
        public int Limit { get; } = initialLimit;

        public int Remaining { get; private set; } = initialRemain;

        public float ResetAfter { get; private set; } = resetAfter;

        public float Reset { get; private set; } = reset;

        public bool Validate() => Remaining > 0 && Reset > DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public void Update(RateLimit rateLimit)
        {
            Remaining = rateLimit.Remaining;
            ResetAfter = rateLimit.ResetAfter;
            Reset = rateLimit.Reset;
        }

        public float EnqueueTime() => Reset - DateTimeOffset.UtcNow.ToUnixTimeSeconds() + (Discord.Random.Next(1, 10) / 5);

        public float Requested() => Remaining--;
    }
}

using System;

namespace SimpleDiscord.Networking
{
    internal class RateLimit(string uri, int initialLimit, int initialRemain, float resetAfter, float reset, bool def = false)
    {
        public string Uri { get; } = uri;

        public int Limit { get; } = initialLimit;

        public int Remaining { get; private set; } = initialRemain;

        public float ResetAfter { get; private set; } = resetAfter;

        public float Reset { get; private set; } = reset;

        public bool Default { get; private set; } = def;

        public bool Validate() => Remaining > 0;

        public float EnqueueTime() => Reset - DateTimeOffset.UtcNow.ToUnixTimeSeconds() + (Discord.Random.Next(1, 10) / 2);

        public float Requested() => Remaining--;
    }
}

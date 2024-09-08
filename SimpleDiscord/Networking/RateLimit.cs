using System;

namespace SimpleDiscord.Networking
{
    internal class RateLimit(string uri, int initialLimit, int initialRemain, decimal resetAfter, decimal reset, RateLimitHandler instance, bool def = false)
    {
        public readonly string id = Guid.NewGuid().ToString();

        public string Uri { get; } = uri;

        public int Limit { get; } = initialLimit;

        public int Remaining { get; private set; } = initialRemain;

        public decimal ResetAfter { get; private set; } = resetAfter;

        public decimal Reset { get; private set; } = reset;

        public bool Default { get; private set; } = def;

        internal string RawReset { get; set; }

        private readonly RateLimitHandler instance = instance;

        public bool Validate(out decimal waitingTime)
        {
            waitingTime = 0m;
            if (Remaining > 0)
                return true;

            if (Remaining < 0)
            {
                waitingTime = instance.GetExpectedTryAgainTime(Uri) * 1000 + 50;
                return false; // Already smth is moving
            }

            waitingTime = EnqueueTime() * 1000 + 35;
            return Reset < DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public decimal EnqueueTime() => Reset - DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public float Requested() => Remaining--;
    }
}

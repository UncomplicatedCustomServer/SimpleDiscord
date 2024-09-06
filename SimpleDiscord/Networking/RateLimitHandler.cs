using SimpleDiscord.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;

namespace SimpleDiscord.Networking
{
    internal class RateLimitHandler()
    {
        private readonly Dictionary<string, RateLimit> EndpointRateLimits = [];

        private readonly Dictionary<string, float> WaitingTime = [];

        private readonly Dictionary<string, float> CalculatedTime = [];

        public RateLimit GetRateLimit(HttpRequestMessage request)
        {
            if (EndpointRateLimits.TryGetValue(GenerateUri(request), out RateLimit rateLimit))
                return rateLimit;
            else
            {
                RateLimit rateLimit1 = new(GenerateUri(request), 1, 1, 0.75f, DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 1.75f, true);
                EndpointRateLimits[GenerateUri(request)] = rateLimit1;
                return rateLimit1;
            }
        }

        public float GetWaitingTime(HttpRequestMessage request, string id)
        {
            string uri = GenerateUri(request);
            RateLimit rateLimit = GetRateLimit(request);
            if (WaitingTime.ContainsKey(uri))
                WaitingTime[uri] += rateLimit.EnqueueTime();
            else
                WaitingTime.Add(uri, rateLimit.EnqueueTime());

            CalculatedTime.Add(id, rateLimit.EnqueueTime());

            return WaitingTime[uri];
        }

        public void ResolveWaitingTime(HttpRequestMessage request, string id)
        {
            if (id == string.Empty)
                return;

            string uri = GenerateUri(request);
            if (WaitingTime.ContainsKey(uri))
                WaitingTime[GenerateUri(request)] -= CalculatedTime[id];

            CalculatedTime.Remove(id); // Garbage collector
        }

        public void MakeRequest(RateLimit rateLimit)
        {
            rateLimit.Requested();
            EndpointRateLimits[rateLimit.Uri] = rateLimit;
        }

        public void UpdateRateLimit(HttpRequestMessage request, HttpResponseHeaders headers)
        {
            string rateLimitBucket = string.Empty;
            string rateLimitLimit = string.Empty;
            string rateLimitRemaining = string.Empty;
            string rateLimitReset = string.Empty;
            string rateLimitResetAfter = string.Empty;

            if (headers.TryGetValues("x-ratelimit-bucket", out IEnumerable<string> bucketValues))
                rateLimitBucket = bucketValues.FirstOrDefault();

            if (headers.TryGetValues("x-ratelimit-limit", out var limitValues))
                rateLimitLimit = limitValues.FirstOrDefault();

            if (headers.TryGetValues("x-ratelimit-remaining", out var remainingValues))
                rateLimitRemaining = remainingValues.FirstOrDefault();

            if (headers.TryGetValues("x-ratelimit-reset", out var resetValues))
                rateLimitReset = resetValues.FirstOrDefault();

            if (headers.TryGetValues("x-ratelimit-reset-after", out var resetAfterValues))
                rateLimitResetAfter = resetAfterValues.FirstOrDefault();

            if (!(rateLimitBucket != string.Empty && rateLimitLimit != string.Empty && rateLimitRemaining != string.Empty && rateLimitReset != string.Empty && rateLimitResetAfter != string.Empty))
                return;

            string uri = GenerateUri(request);
            RateLimit rateLimit = new(uri, int.Parse(rateLimitLimit), int.Parse(rateLimitRemaining), (int)float.Parse(rateLimitResetAfter.Replace('.', ',')), float.Parse(rateLimitReset.Replace('.', ',')));
            EndpointRateLimits[uri] = rateLimit;
        }

        internal static string GenerateUri(HttpRequestMessage msg)
        {
            List<string> elements = [];
            List<string> parts = [.. msg.RequestUri.OriginalString.Replace(Http.Endpoint, string.Empty).Split('/')];

            for (int i = 0; i < parts.Count; i++)
            {
                string part = parts[i];
                if (part is "reactions" && i < parts.Count + 1)
                    parts.Remove(parts[part.IndexOf(part) + 1]);

                if (part is "guilds" && i < part.Length + 1)
                    parts[i + 1] += "_guild";

                if (!long.TryParse(part, out _))
                    elements.Add(part);
            }

            return $"{msg.Method.Method}@{string.Join("/", elements)}";
        }
    }
}

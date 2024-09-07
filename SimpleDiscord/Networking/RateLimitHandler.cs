using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SimpleDiscord.Networking
{
    internal class RateLimitHandler()
    {
        private readonly Dictionary<string, RateLimit> EndpointRateLimits = [];

        public readonly Dictionary<string, decimal> ExpectedTryAgainTime = [];

        public RateLimit GetRateLimit(HttpRequestMessage request)
        {
            if (EndpointRateLimits.TryGetValue(GenerateUri(request), out RateLimit rateLimit))
                return rateLimit;
            else
            {
                RateLimit rateLimit1 = new(GenerateUri(request), 1, 1, 0.75m, DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 1.75m, this, true);
                EndpointRateLimits[GenerateUri(request)] = rateLimit1;
                return rateLimit1;
            }
        }

        public decimal GetExpectedTryAgainTime(string request)
        {
            if (ExpectedTryAgainTime.TryGetValue(request, out decimal expectedTryAgainTime))
                return expectedTryAgainTime;
            else
                return 0m;
        }

        public void UpdateExcepctedTryAgainTime(string response, decimal time)
        {
            if (!ExpectedTryAgainTime.ContainsKey(response))
                ExpectedTryAgainTime.Add(response, time);
        }


        public void MakeRequest(HttpRequestMessage msg)
        {
            EndpointRateLimits[GenerateUri(msg)].Requested();
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
            Console.WriteLine($"\nWARNING: Updated ratelimit for endpoint {uri}\nReset after {rateLimitReset}");
            RateLimit rateLimit = new(uri, int.Parse(rateLimitLimit), int.Parse(rateLimitRemaining), decimal.Parse(rateLimitResetAfter, new CultureInfo("en-US")), decimal.Parse(rateLimitReset, new CultureInfo("en-US")), this)
            {
                RawReset = rateLimitReset
            };
            UpdateExcepctedTryAgainTime(uri, decimal.Parse(rateLimitResetAfter, new CultureInfo("en-US")));
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

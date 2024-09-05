using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SimpleDiscord.Networking
{
    internal class RateLimitHandler()
    {
        // The key is the url and it's encoded like <METHOD>@<URL> - in this way we can keep trace of X-RateLimit-Bucket
        private readonly Dictionary<string, string> EndpointTokens = [];

        // The key is the X-RateLimit-Bucket
        private readonly Dictionary<string, RateLimit> RateLimits = [];

        public RateLimit GetRateLimit(HttpRequestMessage request)
        {
            if (EndpointTokens.TryGetValue($"{request.Method.Method}@{request.RequestUri.OriginalString}", out string token))
                if (RateLimits.TryGetValue(token, out RateLimit rateLimit))
                    return rateLimit;

            return null;
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

            string uri = $"{request.Method.Method}@{request.RequestUri.OriginalString}";
            RateLimit rateLimit = new(int.Parse(rateLimitLimit), int.Parse(rateLimitRemaining), (int)float.Parse(rateLimitResetAfter.Replace('.', ',')), float.Parse(rateLimitReset.Replace('.', ',')));
            if (RateLimits.TryGetValue($"{request.Method.Method}@{request.RequestUri.OriginalString}", out RateLimit original))
                original.Update(rateLimit);
            else
                RateLimits.Add(uri, rateLimit);
        }
    }
}

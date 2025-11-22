using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBotLib.Core.Networking
{
    public class RateLimitHandler : DelegatingHandler
    {
        private readonly Dictionary<string, RateLimitBucket> buckets;
        private readonly object lockobject;
        private readonly int maxretries;

        public RateLimitHandler(int maxretries = 3)
        {
            buckets = new Dictionary<string, RateLimitBucket>();
            lockobject = new object();
            this.maxretries = maxretries;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string bucketid = GetBucketId(request);
            int retrycount = 0;

            while (retrycount < maxretries)
            {
                RateLimitBucket bucket = GetOrCreateBucket(bucketid);

                if (bucket.IsRateLimited)
                {
                    TimeSpan retryafter = bucket.RetryAfter.Value - DateTime.UtcNow;
                    if (retryafter > TimeSpan.Zero)
                    {
                        await Task.Delay(retryafter, cancellationToken);
                    }
                }

                if (bucket.CanMakeRequest)
                {
                    var response = await base.SendAsync(request, cancellationToken);
                    UpdateBucketFromResponse(bucket, response);

                    if ((int)response.StatusCode == 429)
                    {
                        retrycount++;
                        continue;
                    }

                    return response;
                }
                else
                {
                    await Task.Delay(bucket.GetTimeUntilReset(), cancellationToken);
                }
            }

            throw new HttpRequestException($"Rate limit exceeded after {maxretries} retries");
        }

        private string GetBucketId(HttpRequestMessage request)
        {
            // Use route + major parameters as bucket ID
            string route = request.RequestUri.AbsolutePath;
            return route.ToLowerInvariant();
        }

        private RateLimitBucket GetOrCreateBucket(string bucketid)
        {
            lock (lockobject)
            {
                if (!buckets.ContainsKey(bucketid))
                {
                    buckets[bucketid] = new RateLimitBucket(bucketid);
                }
                return buckets[bucketid];
            }
        }

        private void UpdateBucketFromResponse(RateLimitBucket bucket, HttpResponseMessage response)
        {
            if (response.Headers.Contains("X-RateLimit-Limit") &&
                response.Headers.Contains("X-RateLimit-Remaining") &&
                response.Headers.Contains("X-RateLimit-Reset"))
            {
                int limit = int.Parse(response.Headers.GetValues("X-RateLimit-Limit").First());
                int remaining = int.Parse(response.Headers.GetValues("X-RateLimit-Remaining").First());
                long reset = long.Parse(response.Headers.GetValues("X-RateLimit-Reset").First());

                bucket.UpdateFromHeaders(limit, remaining, reset);
            }

            if (response.Headers.Contains("Retry-After"))
            {
                double retryafter = double.Parse(response.Headers.GetValues("Retry-After").First());
                bucket.SetRateLimit(TimeSpan.FromSeconds(retryafter));
            }
        }
    }

    public class RateLimitBucket
    {
        public string Id { get; }
        public int Limit { get; private set; }
        public int Remaining { get; private set; }
        public DateTime ResetTime { get; private set; }
        public DateTime? RetryAfter { get; private set; }
        public bool IsRateLimited => RetryAfter.HasValue && RetryAfter > DateTime.UtcNow;
        public bool CanMakeRequest => !IsRateLimited && Remaining > 0;

        public RateLimitBucket(string id)
        {
            Id = id;
            Limit = 1;
            Remaining = 1;
            ResetTime = DateTime.UtcNow;
        }

        public void UpdateFromHeaders(int limit, int remaining, long reset)
        {
            Limit = limit;
            Remaining = remaining;
            ResetTime = DateTimeOffset.FromUnixTimeSeconds(reset).UtcDateTime;
        }

        public void SetRateLimit(TimeSpan retryafter)
        {
            RetryAfter = DateTime.UtcNow.Add(retryafter);
        }

        public TimeSpan GetTimeUntilReset()
        {
            return ResetTime - DateTime.UtcNow;
        }

        public void DecrementRemaining()
        {
            if (Remaining > 0)
                Remaining--;
        }
    }
}
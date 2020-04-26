using BetfairMetadataService.Domain.BetfairDtos;
using Polly;
using Polly.Caching;
using Polly.Registry;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace BetfairMetadataService.WebRequests
{
    public static class PollyPolicyRegistration
    {
        private const string _successfulLoginStatus = "SUCCESS";
        private static readonly TimeSpan _timeToLive = TimeSpan.FromMinutes(1);
        private static readonly IEnumerable<TimeSpan> thriceTriplingTimeSpans = new TimeSpan[]
        {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(3),
                TimeSpan.FromSeconds(9)
        };
        public static void GetPolicyRegistry(IAsyncCacheProvider cacheProvider,
            IPolicyRegistry<string> registry)
        {
            registry.Add("thriceTriplingRetryPolicy", Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(thriceTriplingTimeSpans));
            registry.Add("loginResponseRetryPolicy", Policy.HandleResult<LoginResponse>(lr => lr.LoginStatus != _successfulLoginStatus)
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(thriceTriplingTimeSpans));
            registry.Add("thirtySecondTimeoutPolicy", Policy.TimeoutAsync(TimeSpan.FromSeconds(30)));

            AsyncCachePolicy<LoginResponse> cachePolicy = Policy.CacheAsync<LoginResponse>(cacheProvider, _timeToLive);
            registry.Add("oneMinuteLoginCachePolicy", cachePolicy);
        }
    }
}

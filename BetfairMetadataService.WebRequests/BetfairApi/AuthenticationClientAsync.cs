using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.WebRequests.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Polly.Retry;
using Polly.Timeout;
using Polly;
using System.Threading;

namespace BetfairMetadataService.WebRequests.BetfairApi
{
    public class AuthenticationClientAsync : IAuthenticationClientAsync
    {
        private const string _successfulLoginStatus = "SUCCESS";
        private readonly string _url;
        private readonly HttpClient _httpClient;
        private readonly FormUrlEncodedContent _loginBody;
        private const int _timeOutSeconds = 30;
        private static readonly IEnumerable<TimeSpan> _retryDelays = new TimeSpan[] 
        { 
            TimeSpan.FromSeconds(1), 
            TimeSpan.FromSeconds(3), 
            TimeSpan.FromSeconds(9) 
        };

        private static readonly AsyncTimeoutPolicy _timeoutPolicy = Policy.TimeoutAsync(_timeOutSeconds);

        private static readonly AsyncRetryPolicy<HttpResponseMessage> _asyncGetPolicy =
            Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(_retryDelays);

        private static readonly AsyncRetryPolicy<LoginResponse> _readStreamPolicy = Policy.HandleResult<LoginResponse>(lr => lr.LoginStatus != _successfulLoginStatus)
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(_retryDelays);

        public AuthenticationClientAsync(IConfiguration configuration, HttpClient httpClient)
        {
            _url = configuration["BetfairApi:Authentication:Url"];

            var userName = configuration["BetfairApi:Authentication:UserName"];
            var password = configuration["BetfairApi:Authentication:Password"];
            var postData = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", password)
            };
            _loginBody = new FormUrlEncodedContent(postData);

            var appKey = configuration["BetfairApi:AppKey"];
            var baseUrl = configuration["BetfairApi:Authentication:BaseUrl"];
            var appKeyHeader = configuration["BetfairApi:AppKeyHeader"];
            var mediaType = configuration["BetfairApi:Authentication:MediaType"];
            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add(appKeyHeader, appKey);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            _httpClient = httpClient;
        }
        public async Task<LoginResponse> Login()
        {
            return await _readStreamPolicy.ExecuteAsync(async () =>
            {
                HttpResponseMessage responseMessage = await _asyncGetPolicy.ExecuteAsync(async () =>
                    await _timeoutPolicy.ExecuteAsync(async token =>
                        await _httpClient.PostAsync(_url, _loginBody, token), CancellationToken.None));
                string content = await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LoginResponse>(content);
            });
        }
    }
}

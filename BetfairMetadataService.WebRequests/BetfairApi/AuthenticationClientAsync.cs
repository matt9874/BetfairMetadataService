using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.DataAccess.Interfaces.WebRequests;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;
using Polly.Registry;
using Polly;
using System.Web.Http;

namespace BetfairMetadataService.WebRequests.BetfairApi
{
    public class AuthenticationClientAsync : IAuthenticationClientAsync
    {
        private readonly string _url;
        private readonly HttpClient _httpClient;
        private readonly IPolicyRegistry<string> _registry;
        private readonly FormUrlEncodedContent _loginBody;

        public AuthenticationClientAsync(IConfiguration configuration, HttpClient httpClient, IPolicyRegistry<string> registry)
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
            _registry = registry;
        }
        public async Task<LoginResponse> Login()
        {
            var timeoutPolicy = _registry.Get<IAsyncPolicy>("thirtySecondTimeoutPolicy");
            var asyncGetPolicy = _registry.Get<IAsyncPolicy<HttpResponseMessage>>("thriceTriplingRetryPolicy");
            var readStreamPolicy = _registry.Get<IAsyncPolicy<LoginResponse>>("loginResponseRetryPolicy");
            var cachePolicy = _registry.Get<IAsyncPolicy<LoginResponse>>("oneMinuteLoginCachePolicy");
            Context policyExecutionContext = new Context($"AuthLogin");

            return await cachePolicy.ExecuteAsync(async context => 
                await readStreamPolicy.ExecuteAsync(async () =>
                {
                    HttpResponseMessage responseMessage = await asyncGetPolicy.ExecuteAsync(async () =>
                        await timeoutPolicy.ExecuteAsync(async token =>
                            await _httpClient.PostAsync(_url, _loginBody, token), CancellationToken.None));
                    if (!responseMessage.IsSuccessStatusCode)
                        throw new HttpResponseException(responseMessage);
                    string content = await responseMessage.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<LoginResponse>(content);
                }),
                policyExecutionContext
            );
        }
    }
}

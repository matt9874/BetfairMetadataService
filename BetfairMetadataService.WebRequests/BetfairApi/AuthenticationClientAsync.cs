using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.WebRequests.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BetfairMetadataService.WebRequests.BetfairApi
{
    public class AuthenticationClientAsync : IAuthenticationClientAsync
    {
        private readonly string _url;
        private readonly string _userName;
        private readonly string _password;

        private readonly IHttpClientFactory _httpClientFactory;

        public AuthenticationClientAsync(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _url = configuration["BetfairApi:Authentication:Url"];
            _userName = configuration["BetfairApi:Authentication:UserName"];
            _password = configuration["BetfairApi:Authentication:Password"];
            _httpClientFactory = httpClientFactory;
        }
        public async Task<LoginResponse> Login()
        {
            HttpClient _httpClient = _httpClientFactory.CreateClient("AuthClient");
            HttpResponseMessage result = await _httpClient.PostAsync(_url, GetLoginBodyAsContent());
            result.EnsureSuccessStatusCode();
            string content = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LoginResponse>(content);
        }

        private FormUrlEncodedContent GetLoginBodyAsContent()
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("username", _userName));
            postData.Add(new KeyValuePair<string, string>("password", _password));
            return new FormUrlEncodedContent(postData);
        }
    }
}

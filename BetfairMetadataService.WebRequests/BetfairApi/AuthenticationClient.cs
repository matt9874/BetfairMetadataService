using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.WebRequests.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace BetfairMetadataService.WebRequests.BetfairApi
{
    public class AuthenticationClient : IAuthenticationClient
    {
        private readonly string _url;
        private readonly string _userName;
        private readonly string _password;

        private readonly IHttpClientFactory _httpClientFactory;

        public AuthenticationClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _url = configuration["BetfairApi:Authentication:Url"];
            _userName = configuration["BetfairApi:Authentication:UserName"];
            _password = configuration["BetfairApi:Authentication:Password"];
            _httpClientFactory = httpClientFactory;
        }
        public LoginResponse Login()
        {
            HttpClient _httpClient = _httpClientFactory.CreateClient("AuthClient");
            HttpResponseMessage result = _httpClient.PostAsync(_url, GetLoginBodyAsContent()).Result;
            result.EnsureSuccessStatusCode();
            var response = JsonConvert.DeserializeObject<LoginResponse>(result.Content.ReadAsStringAsync().Result);

            return response;
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

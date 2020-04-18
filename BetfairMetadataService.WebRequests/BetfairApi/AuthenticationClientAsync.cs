using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.WebRequests.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BetfairMetadataService.WebRequests.BetfairApi
{
    public class AuthenticationClientAsync : IAuthenticationClientAsync
    {
        private readonly string _url;
        private readonly string _userName;
        private readonly string _password;

        private readonly HttpClient _httpClient;

        public AuthenticationClientAsync(IConfiguration configuration, HttpClient httpClient)
        {
            _url = configuration["BetfairApi:Authentication:Url"];
            _userName = configuration["BetfairApi:Authentication:UserName"];
            _password = configuration["BetfairApi:Authentication:Password"]; 
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

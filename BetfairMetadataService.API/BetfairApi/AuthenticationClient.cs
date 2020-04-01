using BetfairMetadataService.API.BetfairApi.Dtos;
using BetfairMetadataService.API.ExternalWebInterfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace BetfairMetadataService.API.BetfairApi
{
    public class AuthenticationClient : IAuthenticationClient
    {
        private const string _certificateFilepath = "AAA";
        private const string _appKey = "BBB";
        private const string _baseUrl = "https://identitysso-cert.betfair.com";
        private const string _url = "/api/certlogin";
        private const string _appKeyHeader = "X-Application";
        private const string _mediaType = "application/json";
        private const string _userName = "CCC";
        private const string _password = "DDD";

        private readonly static HttpClient _httpClient;
        private static readonly FormUrlEncodedContent _content;

        static AuthenticationClient()
        {
            var handler = new HttpClientHandler();
            var cert = new X509Certificate2(_certificateFilepath, "", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            handler.ClientCertificates.Add(cert);
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add(_appKeyHeader, _appKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            _content = GetLoginBodyAsContent();
        }
        public LoginResponse Login()
        {
            HttpResponseMessage result = _httpClient.PostAsync(_url, _content).Result;
            result.EnsureSuccessStatusCode();
            var response = JsonConvert.DeserializeObject<LoginResponse>(result.Content.ReadAsStringAsync().Result);

            return response;
        }

        private static FormUrlEncodedContent GetLoginBodyAsContent()
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("username", _userName));
            postData.Add(new KeyValuePair<string, string>("password", _password));
            return new FormUrlEncodedContent(postData);
        }
    }
}

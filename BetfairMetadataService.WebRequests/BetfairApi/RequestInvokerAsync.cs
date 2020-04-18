using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.WebRequests.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BetfairMetadataService.WebRequests.BetfairApi
{
    public class RequestInvokerAsync: IRequestInvokerAsync
    {
        private readonly string _sessionTokenHeader;
        private readonly string _methodPrefix;
        private readonly string _requestContentType;
        private readonly IAuthenticationClientAsync _authenticationClient;
        private readonly HttpClient _httpClient;
        private Dictionary<string,string> _customHeaders;

        public RequestInvokerAsync(IAuthenticationClientAsync authenticationClient, IConfiguration configuration,
            HttpClient httpClient)
        {
            _authenticationClient = authenticationClient;
            _sessionTokenHeader = configuration["BetfairApi:SessionTokenHeader"];
            _methodPrefix = configuration["BetfairApi:MethodPrefix"];
            _requestContentType = configuration["BetfairApi:ContentType"];
            _customHeaders = new Dictionary<string, string>();
            _customHeaders[configuration["BetfairApi:AppKeyHeader"]] = configuration["BetfairApi:AppKey"];
            httpClient.BaseAddress = new Uri(configuration["BetfairApi:Url"]);

            var acceptableCharsets = new string[] { "ISO-8859-1", "utf-8" };
            foreach (var charset in acceptableCharsets)
                httpClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue(charset));

            ServicePointManager.Expect100Continue = false;
            _httpClient = httpClient;
        }

        public async Task<T> Invoke<T>(string method, IDictionary<string, object> args = null)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            if (method.Length == 0)
                throw new ArgumentException("method cannot be empty string", "method");

            LoginResponse loginResponse = await _authenticationClient.Login();

            if (loginResponse.SessionToken == null)
                throw new Exception("LoginResponse does not contain SessionToken");
            else
                _customHeaders[_sessionTokenHeader] = loginResponse.SessionToken;

            var json = JsonConvert.SerializeObject( new JsonRequest { Method = _methodPrefix + method, Id = 1, Params = args });
            var content = new StringContent(json, Encoding.UTF8, _requestContentType);
            
            foreach(var header in _customHeaders)
                content.Headers.Add(header.Key, header.Value);

            HttpResponseMessage result = await _httpClient.PostAsync("", content);
            result.EnsureSuccessStatusCode();
            var response = JsonConvert.DeserializeObject<JsonResponse<T>>(await result.Content.ReadAsStringAsync());

            if (response.HasError)
            {
                throw ReconstituteException(response.Error);
            }
            else
            {
                return response.Result;
            }
        }
        private static APINGException ReconstituteException(BetfairApiException ex)
        {
            var data = ex.Data;
            var exceptionName = data.Property("exceptionname").Value.ToString();
            var exceptionData = data.Property(exceptionName).Value.ToString();
            return JsonConvert.DeserializeObject<APINGException>(exceptionData);
        }
    }
}

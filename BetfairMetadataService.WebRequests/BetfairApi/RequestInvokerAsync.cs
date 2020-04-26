using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.WebRequests.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using Polly.Retry;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
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
        private readonly IPolicyRegistry<string> _registry;
        private Dictionary<string,string> _customHeaders;

        private static readonly IEnumerable<TimeSpan> _retryDelays = new TimeSpan[]
        {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(3),
            TimeSpan.FromSeconds(9)
        }; 
        
        public RequestInvokerAsync(IAuthenticationClientAsync authenticationClient, IConfiguration configuration,
            HttpClient httpClient, IPolicyRegistry<string> registry)
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
            _registry = registry;
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

            var timeoutPolicy = _registry.Get<AsyncTimeoutPolicy>("thirtySecondTimeoutPolicy");
            var asyncGetPolicy = _registry.Get<AsyncRetryPolicy<HttpResponseMessage>>("thriceTriplingRetryPolicy");
            AsyncRetryPolicy<JsonResponse<T>> readStreamPolicy = Policy.HandleResult<JsonResponse<T>>(jr => jr.HasError)
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(_retryDelays);

            JsonResponse<T> response = await readStreamPolicy.ExecuteAsync(async () =>
            {
                HttpResponseMessage responseMessage = await asyncGetPolicy.ExecuteAsync(async () =>
                    await timeoutPolicy.ExecuteAsync(async token =>
                        await _httpClient.PostAsync("", content, token), CancellationToken.None));
                string responseContent = await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse<T>>(responseContent);
            });

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
            var exceptionName = data?.Property("exceptionname").Value.ToString();
            var exceptionData = (data==null||exceptionName==null) ? null : data.Property(exceptionName).Value.ToString();
            return exceptionData == null ? new APINGException() : JsonConvert.DeserializeObject<APINGException>(exceptionData);
        }
    }
}

using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.WebRequests.BetfairApi;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Polly;
using Polly.Registry;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BetfairMetadataService.WebRequests.Tests.BetfairApiTests
{
    [TestClass]
    public class AuthenticationClientAsyncTests
    {
        private static readonly IAsyncPolicy _noOpPolicy = Policy.NoOpAsync();
        private static readonly IAsyncPolicy<HttpResponseMessage> _noOpHttpResponsePolicy =
            Policy.NoOpAsync<HttpResponseMessage>();
        private static readonly IAsyncPolicy<LoginResponse> _noOpLoginResponsePolicy =
            Policy.NoOpAsync<LoginResponse>();

        private Mock<IConfiguration> _mockConfiguration;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private IPolicyRegistry<string> _mockRegistry;

        [TestInitialize]
        public void TestInit()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["BetfairApi:Authentication:BaseUrl"])
                .Returns("http://baseUrl.com");
            _mockConfiguration.Setup(c => c["BetfairApi:Authentication:MediaType"])
                .Returns("application/json");
            _mockConfiguration.Setup(c => c["BetfairApi:AppKeyHeader"])
                .Returns("header");
            _mockConfiguration.Setup(c => c["BetfairApi:AppKey"])
                .Returns("key");

            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            _mockRegistry = new PolicyRegistry();
            _mockRegistry.Add("thirtySecondTimeoutPolicy", _noOpPolicy);
            _mockRegistry.Add("thriceTriplingRetryPolicy", _noOpHttpResponsePolicy);
            _mockRegistry.Add("loginResponseRetryPolicy", _noOpLoginResponsePolicy);
            _mockRegistry.Add("oneMinuteLoginCachePolicy", _noOpLoginResponsePolicy);
        }

        [TestMethod]
        public async Task Login_InternalServerError_ThrowsHttpResponseException()
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage()
                { 
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("{}")
                }));

            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            var authClient = new AuthenticationClientAsync(_mockConfiguration.Object, httpClient, _mockRegistry);

            await Assert.ThrowsExceptionAsync<HttpResponseException>(async() => await authClient.Login());
        }

        [TestMethod]
        public async Task Login_InternalServerError_SessionTokenIsNull()
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("{}")
                }));

            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            var authClient = new AuthenticationClientAsync(_mockConfiguration.Object, httpClient, _mockRegistry);

            try
            {
                await authClient.Login();
            }
            catch (HttpResponseException exception)
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, exception.Response.StatusCode);
            }
        }


        [TestMethod]
        public async Task Login_Success_SessionTokenIsNotNull()
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"{""loginStatus"": ""SUCCESS"", ""sessionToken"": ""abc""}")
                }));

            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            var authClient = new AuthenticationClientAsync(_mockConfiguration.Object, httpClient, _mockRegistry);

            var loginResponse = await authClient.Login();

            Assert.IsNotNull(loginResponse.SessionToken);
        }

        [TestMethod]
        public async Task Login_Success_LoginStatusIsSuccess()
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"{""loginStatus"": ""SUCCESS"", ""sessionToken"": ""abc""}")
                }));

            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            var authClient = new AuthenticationClientAsync(_mockConfiguration.Object, httpClient, _mockRegistry);

            var loginResponse = await authClient.Login();

            Assert.AreEqual("SUCCESS", loginResponse.LoginStatus);
        }

        [TestMethod]
        public async Task Login_Failed_SessionTokenIsNull()
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"{""loginStatus"":""INVALID_USERNAME_OR_PASSWORD""}")
                }));

            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            var authClient = new AuthenticationClientAsync(_mockConfiguration.Object, httpClient, _mockRegistry);

            var loginResponse = await authClient.Login();

            Assert.IsNull(loginResponse.SessionToken);
        }

        [TestMethod]
        public async Task Login_Failed_LoginStatusIsNotSuccess()
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"{""loginStatus"":""INVALID_USERNAME_OR_PASSWORD""}")
                }));

            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            var authClient = new AuthenticationClientAsync(_mockConfiguration.Object, httpClient, _mockRegistry);

            var loginResponse = await authClient.Login();

            Assert.AreNotEqual("SUCCESS", loginResponse.LoginStatus);
        }
    }
}

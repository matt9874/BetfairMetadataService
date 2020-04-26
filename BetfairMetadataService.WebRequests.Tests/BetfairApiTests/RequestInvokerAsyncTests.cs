using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.WebRequests.BetfairApi;
using BetfairMetadataService.WebRequests.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Polly;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BetfairMetadataService.WebRequests.Tests.BetfairApiTests
{
    [TestClass]
    public class RequestInvokerAsyncTests
    {
        private static readonly IAsyncPolicy _noOpPolicy = Policy.NoOpAsync();
        private static readonly IAsyncPolicy<HttpResponseMessage> _noOpHttpResponsePolicy =
            Policy.NoOpAsync<HttpResponseMessage>();

        private Mock<IConfiguration> _mockConfiguration;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private IPolicyRegistry<string> _mockRegistry;
        private RequestInvokerAsync _requestInvoker;

        [TestInitialize]
        public void TestInit()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["BetfairApi:Url"])
                .Returns("http://notarealurl.fab");
            _mockConfiguration.Setup(c => c["BetfairApi:AppKeyHeader"])
                .Returns("appkeyheader");
            _mockConfiguration.Setup(c => c["BetfairApi:SessionTokenHeader"])
                .Returns("sessiontokenheader");

            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            _mockRegistry = new PolicyRegistry();
            _mockRegistry.Add("thirtySecondTimeoutPolicy", _noOpPolicy);
            _mockRegistry.Add("thriceTriplingRetryPolicy", _noOpHttpResponsePolicy);
        }

        [TestMethod]
        public async Task Invoke_MethodIsNull_ThrowsArgumentNullException()
        {
            var mockAuthenticationClient = new Mock<IAuthenticationClientAsync>();

            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            
            _requestInvoker = new RequestInvokerAsync(mockAuthenticationClient.Object, _mockConfiguration.Object, httpClient, _mockRegistry);
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async() => 
                await _requestInvoker.Invoke<IList<EventTypeResult>>(null));
        }

        [TestMethod]
        public async Task Invoke_MethodIsEmpty_ThrowsArgumentOutOfRangeException()
        {
            var mockAuthenticationClient = new Mock<IAuthenticationClientAsync>();
            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            _requestInvoker = new RequestInvokerAsync(mockAuthenticationClient.Object, _mockConfiguration.Object, httpClient, _mockRegistry);
            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () =>
                await _requestInvoker.Invoke<IList<EventTypeResult>>(string.Empty));
        }

        [TestMethod]
        public async Task Invoke_AuthenticationThrowsHttpResponseException_ThrowsHttpResponseException()
        {
            var mockAuthenticationClient = new Mock<IAuthenticationClientAsync>();
            mockAuthenticationClient.Setup(ac => ac.Login())
                .Throws( new HttpResponseException(HttpStatusCode.InternalServerError));

            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            _requestInvoker = new RequestInvokerAsync(mockAuthenticationClient.Object, _mockConfiguration.Object, httpClient, _mockRegistry);
            await Assert.ThrowsExceptionAsync<HttpResponseException>(async () =>
                await _requestInvoker.Invoke<IList<EventTypeResult>>("listEventTypes"));
        }

        [TestMethod]
        public async Task Invoke_LoginResponseIsNull_ThrowsAuthenticationException()
        {
            var mockAuthenticationClient = new Mock<IAuthenticationClientAsync>();
            mockAuthenticationClient.Setup(ac => ac.Login())
                .Returns(Task.FromResult((LoginResponse)null));

            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            _requestInvoker = new RequestInvokerAsync(mockAuthenticationClient.Object, _mockConfiguration.Object, httpClient, _mockRegistry);
            await Assert.ThrowsExceptionAsync<AuthenticationException>(async () =>
                await _requestInvoker.Invoke<IList<EventTypeResult>>("listEventTypes"));
        }

        [TestMethod]
        public async Task Invoke_SessionTokenIsNull_ThrowsAuthenticationException()
        {
            var mockAuthenticationClient = new Mock<IAuthenticationClientAsync>();
            mockAuthenticationClient.Setup(ac => ac.Login())
                .Returns(Task.FromResult(new LoginResponse() {LoginStatus="FAILED" }));

            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            _requestInvoker = new RequestInvokerAsync(mockAuthenticationClient.Object, _mockConfiguration.Object, httpClient, _mockRegistry);
            await Assert.ThrowsExceptionAsync<AuthenticationException>(async () =>
                await _requestInvoker.Invoke<IList<EventTypeResult>>("listEventTypes"));
        }

        [TestMethod]
        public async Task Invoke_HttpInternalServerError_ThrowsHttpResponseException()
        {
            var mockAuthenticationClient = new Mock<IAuthenticationClientAsync>();
            mockAuthenticationClient.Setup(ac => ac.Login())
                .Returns(Task.FromResult(new LoginResponse() { LoginStatus = "SUCCESS", SessionToken="abc" }));

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("{}")
                }));

            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            _requestInvoker = new RequestInvokerAsync(mockAuthenticationClient.Object, _mockConfiguration.Object, httpClient, _mockRegistry);
            await Assert.ThrowsExceptionAsync<HttpResponseException>(async () =>
                await _requestInvoker.Invoke<IList<EventTypeResult>>("listEventTypes"));
        }

        [TestMethod]
        public async Task Invoke_HttpInternalServerError_HttpResponseExceptionHasInternalServerError()
        {
            var mockAuthenticationClient = new Mock<IAuthenticationClientAsync>();
            mockAuthenticationClient.Setup(ac => ac.Login())
                .Returns(Task.FromResult(new LoginResponse() { LoginStatus = "SUCCESS", SessionToken = "abc" }));

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("{}")
                }));

            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            _requestInvoker = new RequestInvokerAsync(mockAuthenticationClient.Object, _mockConfiguration.Object, httpClient, _mockRegistry);
            try
            {
                await _requestInvoker.Invoke<IList<EventTypeResult>>("listEventTypes");
            }
            catch (HttpResponseException exception)
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, exception.Response.StatusCode);
            }
        }
    }
}

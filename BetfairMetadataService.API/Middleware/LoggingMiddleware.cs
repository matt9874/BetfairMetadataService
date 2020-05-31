using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BetfairMetadataService.API.Middleware
{
    public class LoggingMiddleware
    {
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string request = await FormatRequest(context.Request);
            _logger.LogInformation(request);

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                string response = await FormatResponse(context.Response);

                _logger.LogInformation(response);

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            string bodyAsText = Encoding.UTF8.GetString(buffer);

            request.Body.Seek(0, SeekOrigin.Begin);

            return $@"
Trace Id: {request.HttpContext.TraceIdentifier}
Remote IP address: {request.HttpContext.Connection.RemoteIpAddress.ToString()}
Local IP address: {request.HttpContext.Connection.LocalIpAddress.ToString()}
Scheme: {request.Scheme}
Host: {request.Host}
Path: {request.Path}
Query string: {request.QueryString}
Request body: {bodyAsText}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            string text = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return $@"
Trace id: {response.HttpContext.TraceIdentifier}
Status code: {response.StatusCode}
Response body:{text}";
        }
    }
}

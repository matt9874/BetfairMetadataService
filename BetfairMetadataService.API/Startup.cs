using BetfairMetadataService.API.Workers;
using BetfairMetadataService.WebRequests.BetfairApi;
using BetfairMetadataService.WebRequests.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace BetfairMetadataService.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHostedService<MetadataFetchWorker>();
            services.AddSingleton<IAuthenticationClient, AuthenticationClient>();
            services.AddSingleton<IRequestInvoker, RequestInvoker>();

            services.AddHttpClient("AuthClient", client =>
            {
                var appKey = Configuration["BetfairApi:AppKey"];
                var baseUrl = Configuration["BetfairApi:Authentication:BaseUrl"];
                var appKeyHeader = Configuration["BetfairApi:AppKeyHeader"];
                var mediaType = Configuration["BetfairApi:Authentication:MediaType"];
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add(appKeyHeader, appKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            }).ConfigurePrimaryHttpMessageHandler(h =>
            {
                var handler = new HttpClientHandler();
                var certificateFilepath = Configuration["BetfairApi:Authentication:CertificateFilepath"];
                var cert = new X509Certificate2(certificateFilepath, "", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
                handler.ClientCertificates.Add(cert);
                return handler;
            });

            services.AddHttpClient("SportsAPING", client =>
            {
                client.BaseAddress = new Uri(Configuration["BetfairApi:Url"]);
                var acceptableCharsets = new string[] { "ISO-8859-1", "utf-8" };
                foreach (var charset in acceptableCharsets)
                    client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue(charset));
                ServicePointManager.Expect100Continue = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

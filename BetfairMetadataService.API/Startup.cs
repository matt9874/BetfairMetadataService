using AutoMapper;
using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.External;
using BetfairMetadataService.Domain.FetchRoots;
using BetfairMetadataService.SqlServer;
using BetfairMetadataService.SqlServer.FetchRoots;
using BetfairMetadataService.WebRequests;
using BetfairMetadataService.WebRequests.BetfairApi;
using BetfairMetadataService.WebRequests.BetfairApi.Readers;
using BetfairMetadataService.WebRequests.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly.Caching;
using Polly.Caching.Memory;
using Polly.Registry;
using System;
using System.Net.Http;
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
            services.AddControllers(setupAction=>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            }).AddXmlDataContractSerializerFormatters();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMemoryCache();
            services.AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>();
            IPolicyRegistry<string> registry = services.AddPolicyRegistry();

            services.AddHttpClient<IAuthenticationClientAsync, AuthenticationClientAsync>().ConfigurePrimaryHttpMessageHandler(h =>
            {
                var handler = new HttpClientHandler();
                var certificateFilepath = Configuration["BetfairApi:Authentication:CertificateFilepath"];
                var cert = new X509Certificate2(certificateFilepath, "", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
                handler.ClientCertificates.Add(cert);
                return handler;
            });
            services.Configure<DataProviderMappings>((settings) =>
            {
                Configuration.GetSection("DataProviders").Bind(settings);
            });
            services.AddHttpClient<IRequestInvokerAsync, RequestInvokerAsync>();
            string connectionString = Configuration["ConnectionStrings:BetfairMetadataServiceDb"];
            services.AddDbContext<BetfairMetadataServiceContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<IBatchReader<DataProvider>, ConfigurationBatchDataProviderReader>();
            services.AddScoped<IReader<DataProvider, int>, ConfigurationDataProviderReader>();
            services.AddScoped<IReader<EventType, string>, BetfairEventTypeReader>();
            services.AddScoped<IReader<Competition, string>, BetfairCompetitionReader>();
            services.AddScoped<Func<int, IBatchReader<EventType>>>(sp =>
                dataProviderId =>
                {
                    switch (dataProviderId)
                    {
                        case (1):
                            return new BetfairEventTypesBatchReader(sp.GetRequiredService<IRequestInvokerAsync>(), sp.GetRequiredService<IMapper>());
                        default:
                            return new BetfairEventTypesBatchReader(sp.GetRequiredService<IRequestInvokerAsync>(), sp.GetRequiredService<IMapper>());
                    }
                });
            services.AddScoped<Func<int, IBatchReader<Competition>>>(sp =>
                dataProviderId =>
                {
                    switch (dataProviderId)
                    {
                        case (1):
                            return new BetfairCompetitionsBatchReader(sp.GetRequiredService<IRequestInvokerAsync>(), sp.GetRequiredService<IMapper>());
                        default:
                            return new BetfairCompetitionsBatchReader(sp.GetRequiredService<IRequestInvokerAsync>(), sp.GetRequiredService<IMapper>());
                    }
                });

            services.AddScoped<Func<int, IMarketTypesService>>(sp =>
                dataProviderId =>
                {
                    switch (dataProviderId)
                    {
                        case (1):
                            return new MarketTypesService(sp.GetRequiredService<IRequestInvokerAsync>(), sp.GetRequiredService<IMapper>());
                        default:
                            return new MarketTypesService(sp.GetRequiredService<IRequestInvokerAsync>(), sp.GetRequiredService<IMapper>());
                    }
                });

            services.AddScoped<IDeleter<EventTypeMarketType>, EventTypeMarketTypeFetchRootDeleter>();
            services.AddScoped<ISaver<EventTypeMarketType>, EventTypeMarketTypeFetchRootSaver>();
            services.AddScoped<IReader<EventTypeMarketType, Tuple<int,string,string>>, EventTypeMarketTypeFetchRootReader>();
            services.AddScoped<IBatchReader<EventTypeMarketType>, EventTypeMarketTypeFetchRootBatchReader>();

            services.AddScoped<IDeleter<CompetitionMarketType>, CompetitionMarketTypeFetchRootDeleter>();
            services.AddScoped<ISaver<CompetitionMarketType>, CompetitionMarketTypeFetchRootSaver>();
            services.AddScoped<IReader<CompetitionMarketType, Tuple<int, string, string>>, CompetitionMarketTypeFetchRootReader>();
            services.AddScoped<IBatchReader<CompetitionMarketType>, CompetitionMarketTypeFetchRootBatchReader>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAsyncCacheProvider cacheProvider,
            IPolicyRegistry<string> registry)
        {
            PollyPolicyRegistration.GetPolicyRegistry(cacheProvider, registry);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        //TO DO: Add logging
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected error happened");
                    });
                });
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

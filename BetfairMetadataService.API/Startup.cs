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
using BetfairMetadataService.DataAccess.Interfaces.Repositories;
using BetfairMetadataService.DataAccess.Interfaces.WebRequests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly.Caching;
using Polly.Caching.Memory;
using Polly.Registry;
using System;
using BetfairMetadataService.WebRequests.BetfairApi.Repositories;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using BetfairMetadataService.API.Workers;
using BetfairMetadataService.API.WorkerInterfaces;
using BetfairMetadataService.API.Middleware;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;

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
            }).AddXmlDataContractSerializerFormatters()
            .ConfigureApiBehaviorOptions(options => 
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    ProblemDetailsFactory problemDetailsFactory = context.HttpContext.RequestServices
                        .GetRequiredService<ProblemDetailsFactory>();
                    var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                            context.HttpContext,context.ModelState);
                    problemDetails.Instance = context.HttpContext.Request.Path;

                    var actionExecutingContext = context as ActionExecutingContext;

                    if ((context.ModelState.ErrorCount > 0) &&
                        (actionExecutingContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count))
                    {
                        problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                        problemDetails.Title = "One or more validation errors occurred.";
                        return new UnprocessableEntityObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    }
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "One or more errors on input occurred.";
                    return new BadRequestObjectResult(problemDetails)
                    { 
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });

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

            services.AddScoped<IBetfairBatchReader<EventType>, BetfairEventTypesBatchReader>();
            services.AddScoped<IBetfairBatchReader<Competition>, BetfairCompetitionsBatchReader>();
            services.AddScoped<IBetfairBatchReader<Event>, BetfairEventsBatchReader>();
            services.AddScoped<IBetfairBatchReader<MarketType>, BetfairMarketTypesBatchReader>();
            services.AddScoped<IBetfairBatchReader<Market>, BetfairMarketsBatchReader>();

            services.AddScoped<Func<int, IExternalEventTypesRepository>>(sp =>
                dataProviderId =>
                {
                    switch (dataProviderId)
                    {
                        case (1):
                            return new BetfairEventTypesRepository(sp.GetRequiredService<IBetfairBatchReader<EventType>>());
                        default:
                            return null;
                    }
                });
            services.AddScoped<Func<int, IExternalCompetitionsRepository>>(sp =>
                dataProviderId =>
                {
                    switch (dataProviderId)
                    {
                        case (1):
                            return new BetfairCompetitionsRepository(sp.GetRequiredService<IBetfairBatchReader<Competition>>());
                        default:
                            return null;
                    }
                });

            services.AddScoped<Func<int, IExternalEventsRepository>>(sp =>
                dataProviderId =>
                {
                    switch (dataProviderId)
                    {
                        case (1):
                            return new BetfairEventsRepository(sp.GetRequiredService<IBetfairBatchReader<Event>>());
                        default:
                            return null;
                    }
                });

            services.AddScoped<Func<int, IExternalMarketTypesRepository>>(sp =>
                dataProviderId =>
                {
                    switch (dataProviderId)
                    {
                        case (1):
                            return new BetfairMarketTypesRepository(sp.GetRequiredService<IBetfairBatchReader<MarketType>>());
                        default:
                            return null;
                    }
                });

            services.AddScoped<Func<int, IExternalMarketsRepository>>(sp =>
                dataProviderId =>
                {
                    switch (dataProviderId)
                    {
                        case (1):
                            return new BetfairMarketsRepository(sp.GetRequiredService<IBetfairBatchReader<Market>>());
                        default:
                            return null;
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

            services.AddScoped<IEntityUpserter, EntityUpserter>();

            services.AddScoped<BetfairCompetitionsWorker>();
            services.AddScoped<IWorker>(sp => new CompositeWorker(
                new IWorker[]
                {
                    sp.GetRequiredService<BetfairCompetitionsWorker>()
                }));
            services.AddScoped<ICompetitionMarketTypeWorker, BetfairCompetitionWorker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAsyncCacheProvider cacheProvider,
            IPolicyRegistry<string> registry, ILogger<Startup> logger)
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
                        var exceptionHandlerPathFeature =
                            context.Features.Get<IExceptionHandlerPathFeature>();
                        string errorInformation = GetErrorInformation(context);
                        logger.LogError(exceptionHandlerPathFeature?.Error, errorInformation);

                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected error happened");
                    });
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<LoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private string GetErrorInformation(HttpContext context)
        {
            return $@"
Error.Unhandled exception. TraceId: { context.TraceIdentifier}. 
Remote IP address: {context.Connection.RemoteIpAddress.ToString()}
Local IP address: {context.Connection.LocalIpAddress.ToString()}
Request:
    Scheme: {context.Request.Scheme}
    Host: {context.Request.Host}
    Path: {context.Request.Path}
    Query string: {context.Request.QueryString} 
Response:
    Status code: {context.Response.StatusCode}
";
        }
    }
}

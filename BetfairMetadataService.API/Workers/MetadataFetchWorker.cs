using BetfairMetadataService.API.BetfairApi.Dtos;
using BetfairMetadataService.API.ExternalWebInterfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BetfairMetadataService.API.Workers
{
    public class MetadataFetchWorker : IHostedService, IDisposable
    {
        private static readonly TimeSpan _workerStartTimeSpan = TimeSpan.FromMinutes(5);
        private readonly IAuthenticationClient _authenticationClient;

        public MetadataFetchWorker(IAuthenticationClient authenticationClient)
        {
            _authenticationClient = authenticationClient;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                await FetchMetadata();
                await Task.Delay(_workerStartTimeSpan);
            }
        }

        private Task FetchMetadata()
        {
            LoginResponse loginResponse = _authenticationClient.Login();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            
        }

    }
}

using BetfairMetadataService.API.WorkerInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BetfairMetadataService.API.Workers
{
    public class MetadataFetchWorker : IHostedService, IDisposable
    {
        private static readonly TimeSpan _workerStartTimeSpan = TimeSpan.FromSeconds(5);
        private readonly IServiceScopeFactory _scopeFactory;

        public MetadataFetchWorker(IServiceScopeFactory scopeFactory)
        {
            //TODO: get worker time interval from config
            _scopeFactory = scopeFactory;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                await FetchMetadata();
                await Task.Delay(_workerStartTimeSpan);
            }
        }

        private async Task FetchMetadata()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var worker = scope.ServiceProvider.GetRequiredService<IWorker>();
                await worker.DoWork();
            }
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

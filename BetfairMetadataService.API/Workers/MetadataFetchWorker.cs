using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.WebRequests.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BetfairMetadataService.API.Workers
{
    public class MetadataFetchWorker : IHostedService, IDisposable
    {
        private static readonly TimeSpan _workerStartTimeSpan = TimeSpan.FromSeconds(5);
        private readonly IRequestInvoker _requestInvoker;

        public MetadataFetchWorker(IRequestInvoker requestInvoker)
        {
            _requestInvoker = requestInvoker;
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
            var x = _requestInvoker.Invoke<IList<EventTypeResult>>("listEventTypes", new Dictionary<string, object>()
            {
                {"filter", new MarketFilter() }
            });
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

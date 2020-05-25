using BetfairMetadataService.API.WorkerInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetfairMetadataService.API.Workers
{
    public class CompositeWorker : IWorker
    {
        private readonly IEnumerable<IWorker> _workers;

        public CompositeWorker(IEnumerable<IWorker> workers)
        {
            _workers = workers;
        }
        public async Task DoWork()
        {
            foreach (var worker in _workers)
                await worker.DoWork();
        }
    }
}

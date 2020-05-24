using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.DataAccess.Interfaces.Repositories;
using BetfairMetadataService.DataAccess.Interfaces.WebRequests;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BetfairMetadataService.WebRequests.BetfairApi.Repositories
{
    public class BetfairEventTypesRepository : IExternalEventTypesRepository
    {
        private IBetfairBatchReader<Domain.External.EventType> _betfairBatchReader;

        public BetfairEventTypesRepository(IBetfairBatchReader<Domain.External.EventType> betfairBatchReader)
        {
            _betfairBatchReader = betfairBatchReader;
        }

        public async Task<IEnumerable<Domain.External.EventType>> GetEventTypes()
        {
            return await _betfairBatchReader.Read(null);
        }

        public async Task<Domain.External.EventType> GetEventType(string id)
        {
            var filter = new MarketFilter()
            {
                EventTypeIds = new HashSet<string>()
                { 
                    id
                }
            };
            return (await _betfairBatchReader.Read(filter)).FirstOrDefault();
        }
    }
}

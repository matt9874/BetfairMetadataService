using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.Domain.External;
using BetfairMetadataService.DataAccess.Interfaces.Repositories;
using BetfairMetadataService.DataAccess.Interfaces.WebRequests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetfairMetadataService.WebRequests.BetfairApi.Repositories
{
    public class BetfairMarketTypesRepository : IExternalMarketTypesRepository
    {
        private IBetfairBatchReader<MarketType> _betfairBatchReader;

        public BetfairMarketTypesRepository(IBetfairBatchReader<MarketType> betfairBatchReader)
        {
            _betfairBatchReader = betfairBatchReader;
        }

        public async Task<IEnumerable<MarketType>> GetMarketTypesByCompetitionId(string competitionId)
        {
            MarketFilter marketFilter = new MarketFilter()
            {
                CompetitionIds = new HashSet<string>() { competitionId }
            };

            IEnumerable<MarketType> marketTypes = await _betfairBatchReader.Read(marketFilter);
            return marketTypes;
        }

        public async Task<IEnumerable<MarketType>> GetMarketTypesByEventTypeId(string eventTypeId)
        {
            MarketFilter marketFilter = new MarketFilter()
            {
                EventTypeIds = new HashSet<string>() { eventTypeId }
            };

            IEnumerable<MarketType> marketTypes = await _betfairBatchReader.Read(marketFilter);
            return marketTypes;
        }
    }
}

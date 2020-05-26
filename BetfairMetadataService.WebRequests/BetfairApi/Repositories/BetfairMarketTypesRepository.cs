using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.Domain.External;
using BetfairMetadataService.DataAccess.Interfaces.Repositories;
using BetfairMetadataService.DataAccess.Interfaces.WebRequests;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BetfairMetadataService.Domain;

namespace BetfairMetadataService.WebRequests.BetfairApi.Repositories
{
    public class BetfairMarketTypesRepository : IExternalMarketTypesRepository
    {
        private IBetfairBatchReader<MarketType> _betfairBatchReader;

        public BetfairMarketTypesRepository(IBetfairBatchReader<MarketType> betfairBatchReader)
        {
            _betfairBatchReader = betfairBatchReader;
        }

        public async Task<MarketType> GetMarketTypeForCompetition(string competitionId, string marketType)
        {
            IEnumerable<MarketType> marketTypes = await GetMarketTypesByCompetitionId(competitionId);
            return marketTypes?.FirstOrDefault(mt => mt.Name == marketType);
        }

        public async Task<IEnumerable<MarketType>> GetMarketTypesByCompetitionId(string competitionId)
        {
            MarketFilter filter = new MarketFilter()
            {
                CompetitionIds = new HashSet<string>() { competitionId }
            };
            var parameters = new BetfairRequestParameters()
            {
                Filter = filter
            };

            IEnumerable<MarketType> marketTypes = await _betfairBatchReader.Read(parameters);
            return marketTypes;
        }

        public async Task<IEnumerable<MarketType>> GetMarketTypesByEventTypeId(string eventTypeId)
        {
            MarketFilter filter = new MarketFilter()
            {
                EventTypeIds = new HashSet<string>() { eventTypeId }
            };
            var parameters = new BetfairRequestParameters()
            {
                Filter = filter
            };

            IEnumerable<MarketType> marketTypes = await _betfairBatchReader.Read(parameters);
            return marketTypes;
        }
    }
}

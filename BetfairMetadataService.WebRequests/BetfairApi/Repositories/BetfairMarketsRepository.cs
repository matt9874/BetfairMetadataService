using BetfairMetadataService.DataAccess.Interfaces.Repositories;
using BetfairMetadataService.DataAccess.Interfaces.WebRequests;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.Domain.External;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetfairMetadataService.WebRequests.BetfairApi.Repositories
{
    public class BetfairMarketsRepository: IExternalMarketsRepository
    {
        private const int _maxResults = 1000;
        private IBetfairBatchReader<Market> _betfairBatchReader;

        public BetfairMarketsRepository(IBetfairBatchReader<Market> betfairBatchReader)
        {
            _betfairBatchReader = betfairBatchReader;
        }

        public async Task<Market> GetMarketForEventAndMarketType(string eventId, string marketType)
        {
            MarketFilter filter = new MarketFilter()
            {
                EventIds = new HashSet<string>() { eventId },
                MarketTypeCodes = new HashSet<string>() { marketType}
            };
            HashSet<MarketProjection> marketProjections = new HashSet<MarketProjection>()
            { 
                MarketProjection.EVENT,
                MarketProjection.MARKET_DESCRIPTION,
                MarketProjection.RUNNER_DESCRIPTION,
                MarketProjection.RUNNER_METADATA
            };
            var parameters = new BetfairRequestParameters()
            {
                Filter = filter,
                MarketProjections = marketProjections,
                MaxResults = _maxResults
            };

            IEnumerable<Market> marketTypes = await _betfairBatchReader.Read(parameters);
            return marketTypes?.FirstOrDefault(mt => mt.MarketType == marketType);
        }
    }
}

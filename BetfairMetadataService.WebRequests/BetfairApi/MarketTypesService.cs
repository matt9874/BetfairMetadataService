using AutoMapper;
using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.Domain.External;
using BetfairMetadataService.WebRequests.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetfairMetadataService.WebRequests.BetfairApi
{
    public class MarketTypesService : IMarketTypesService
    {
        private readonly IRequestInvokerAsync _requestInvoker;
        private readonly IMapper _mapper;

        public MarketTypesService(IRequestInvokerAsync requestInvoker, IMapper mapper)
        {
            _requestInvoker = requestInvoker;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MarketType>> GetMarketTypesByCompetitionId(string competitionId)
        {
            MarketFilter marketFilter = new MarketFilter()
            {
                CompetitionIds = new HashSet<string>() { competitionId }
            };

            IList<MarketTypeResult> marketTypeResults = await _requestInvoker.Invoke<IList<MarketTypeResult>>(
                BetfairMethod.ListMarketTypes,
                new Dictionary<string, object>()
                {
                    { "filter", marketFilter }
                });
            IEnumerable<MarketType> marketTypes = _mapper.Map<IEnumerable<MarketType>>(marketTypeResults);
            return marketTypes;
        }

        public async Task<IEnumerable<MarketType>> GetMarketTypesByEventTypeId(string eventTypeId)
        {
            MarketFilter marketFilter = new MarketFilter()
            {
                EventTypeIds = new HashSet<string>() { eventTypeId }
            };

            IList<MarketTypeResult> marketTypeResults = await _requestInvoker.Invoke<IList<MarketTypeResult>>(
                BetfairMethod.ListMarketTypes,
                new Dictionary<string, object>()
                {
                    { "filter", marketFilter }
                });

            IEnumerable<MarketType> marketTypes = _mapper.Map<IEnumerable<MarketType>>(marketTypeResults);
            return marketTypes;
        }
    }
}

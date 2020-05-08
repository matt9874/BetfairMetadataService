using AutoMapper;
using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.WebRequests.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetfairMetadataService.WebRequests.BetfairApi.Readers
{
    public class BetfairEventTypeReader : IReader<Domain.External.EventType, string>
    {
        private readonly IRequestInvokerAsync _requestInvoker;
        private readonly IMapper _mapper;

        public BetfairEventTypeReader(IRequestInvokerAsync requestInvoker, IMapper mapper)
        {
            _requestInvoker = requestInvoker;
            _mapper = mapper;
        }

        public async Task<Domain.External.EventType> Read(string id)
        {
            var filter = new MarketFilter() {EventTypeIds = new HashSet<string>() {id } };
            var args = new Dictionary<string, object>()
            {
                {"filter", new MarketFilter() }
            };
            IList<EventTypeResult> results = await _requestInvoker.Invoke<IList<EventTypeResult>>(BetfairMethod.ListEventTypes, args);
            var eventTypes = _mapper.Map<IEnumerable<Domain.External.EventType>>(results);
            return eventTypes.FirstOrDefault();
        }
    }
}

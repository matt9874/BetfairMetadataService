using AutoMapper;
using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.WebRequests.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetfairMetadataService.WebRequests.BetfairApi.Readers
{
    public class BetfairEventTypesBatchReader : IBatchReader<Domain.External.EventType>
    {
        private readonly IRequestInvokerAsync _requestInvoker;
        private readonly IMapper _mapper;

        public BetfairEventTypesBatchReader(IRequestInvokerAsync requestInvoker, IMapper mapper)
        {
            _requestInvoker = requestInvoker;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Domain.External.EventType>> Read(Func<Domain.External.EventType, bool> filter)
        {
            IList<EventTypeResult> results = await _requestInvoker.Invoke<IList<EventTypeResult>>(BetfairMethod.ListEventTypes);
            return _mapper.Map<IEnumerable<Domain.External.EventType>>(results);
        }
    }
}

using AutoMapper;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.DataAccess.Interfaces.WebRequests;

namespace BetfairMetadataService.WebRequests.BetfairApi.Readers
{
    public class BetfairEventTypesBatchReader : AbstractBetfairBatchReader<Domain.External.EventType, EventTypeResult>, 
        IBetfairBatchReader<Domain.External.EventType>
    {
        public BetfairEventTypesBatchReader(IRequestInvokerAsync requestInvoker, IMapper mapper) 
            :base(requestInvoker, mapper)
        {}

        protected override BetfairMethod _method { get { return BetfairMethod.ListEventTypes; } }
    }
}

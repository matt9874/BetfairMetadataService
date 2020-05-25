using AutoMapper;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.DataAccess.Interfaces.WebRequests;

namespace BetfairMetadataService.WebRequests.BetfairApi.Readers
{
    public class BetfairEventsBatchReader : AbstractBetfairBatchReader<Domain.External.Event, EventResult>, 
        IBetfairBatchReader<Domain.External.Event>
    {
        public BetfairEventsBatchReader(IRequestInvokerAsync requestInvoker, IMapper mapper) 
            :base(requestInvoker, mapper)
        {}

        protected override BetfairMethod _method { get { return BetfairMethod.ListEvents; } }
    }
}

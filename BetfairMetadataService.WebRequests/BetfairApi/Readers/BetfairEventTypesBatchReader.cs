using AutoMapper;
using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.WebRequests.Interfaces;

namespace BetfairMetadataService.WebRequests.BetfairApi.Readers
{
    public class BetfairEventTypesBatchReader : AbstractBetfairBatchReader<Domain.External.EventType, EventTypeResult>, 
        IBatchReader<Domain.External.EventType>
    {
        public BetfairEventTypesBatchReader(IRequestInvokerAsync requestInvoker, IMapper mapper) 
            :base(requestInvoker, mapper, BetfairMethod.ListEventTypes)
        {

        }
    }
}

using AutoMapper;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.DataAccess.Interfaces.WebRequests;

namespace BetfairMetadataService.WebRequests.BetfairApi.Readers
{
    public class BetfairMarketTypesBatchReader : AbstractBetfairBatchReader<Domain.External.MarketType, MarketTypeResult>,
        IBetfairBatchReader<Domain.External.MarketType>
    {
        public BetfairMarketTypesBatchReader(IRequestInvokerAsync requestInvoker, IMapper mapper)
            : base(requestInvoker, mapper)
        { }

        protected override BetfairMethod _method { get { return BetfairMethod.ListMarketTypes; } }
    }
}

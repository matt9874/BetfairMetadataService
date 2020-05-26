using AutoMapper;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.DataAccess.Interfaces.WebRequests;

namespace BetfairMetadataService.WebRequests.BetfairApi.Readers
{
    public class BetfairMarketsBatchReader : AbstractBetfairBatchReader<Domain.External.Market, MarketCatalogue>,
        IBetfairBatchReader<Domain.External.Market>
    {
        public BetfairMarketsBatchReader(IRequestInvokerAsync requestInvoker, IMapper mapper)
            : base(requestInvoker, mapper)
        { }

        protected override BetfairMethod _method { get { return BetfairMethod.ListMarketCatalogues; } }
    }
}

using AutoMapper;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.DataAccess.Interfaces.WebRequests;

namespace BetfairMetadataService.WebRequests.BetfairApi.Readers
{
    public class BetfairCompetitionsBatchReader : AbstractBetfairBatchReader<Domain.External.Competition, CompetitionResult>,
        IBetfairBatchReader<Domain.External.Competition>
    {
        public BetfairCompetitionsBatchReader(IRequestInvokerAsync requestInvoker, IMapper mapper)
            : base(requestInvoker, mapper)
        {}

        protected override BetfairMethod _method { get { return BetfairMethod.ListCompetitions; } }
    }
}

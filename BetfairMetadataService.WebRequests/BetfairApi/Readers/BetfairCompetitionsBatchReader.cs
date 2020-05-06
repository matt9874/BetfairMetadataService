using AutoMapper;
using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.WebRequests.Interfaces;

namespace BetfairMetadataService.WebRequests.BetfairApi.Readers
{
    public class BetfairCompetitionsBatchReader : AbstractBetfairBatchReader<Domain.External.Competition, CompetitionResult>,
        IBatchReader<Domain.External.Competition>
    {
        public BetfairCompetitionsBatchReader(IRequestInvokerAsync requestInvoker, IMapper mapper)
            : base(requestInvoker, mapper, BetfairMethod.ListCompetitions)
        {

        }
    }
}

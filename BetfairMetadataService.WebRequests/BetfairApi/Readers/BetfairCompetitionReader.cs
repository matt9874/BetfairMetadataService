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
    public class BetfairCompetitionReader : IReader<Domain.External.Competition, string>
    {
        private readonly IRequestInvokerAsync _requestInvoker;
        private readonly IMapper _mapper;

        public BetfairCompetitionReader(IRequestInvokerAsync requestInvoker, IMapper mapper)
        {
            _requestInvoker = requestInvoker;
            _mapper = mapper;
        }

        public async Task<Domain.External.Competition> Read(string id)
        {
            var filter = new MarketFilter() {CompetitionIds = new HashSet<string>() {id } };
            var args = new Dictionary<string, object>()
            {
                {"filter", filter }
            };
            IList<CompetitionResult> results = await _requestInvoker.Invoke<IList<CompetitionResult>>(BetfairMethod.ListCompetitions, args);
            var competitions = _mapper.Map<IEnumerable<Domain.External.Competition>>(results);
            return competitions.FirstOrDefault();
        }
    }
}

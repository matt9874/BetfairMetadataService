using System.Linq;
using BetfairMetadataService.DataAccess.Interfaces.Repositories;
using BetfairMetadataService.DataAccess.Interfaces.WebRequests;
using System.Collections.Generic;
using System.Threading.Tasks;
using BetfairMetadataService.Domain.BetfairDtos;

namespace BetfairMetadataService.WebRequests.BetfairApi.Repositories
{
    public class BetfairCompetitionsRepository : IExternalCompetitionsRepository
    {
        private IBetfairBatchReader<Domain.External.Competition> _competitionsReader;

        public BetfairCompetitionsRepository(IBetfairBatchReader<Domain.External.Competition> competitionsReader)
        {
            _competitionsReader = competitionsReader;
        }

        public async Task<Domain.External.Competition> GetCompetition(string id)
        {
            var filter = new MarketFilter()
            {
                CompetitionIds = new HashSet<string>()
                {
                    id
                }
            };
            return (await _competitionsReader.Read(filter)).FirstOrDefault();
        }
        public async Task<IEnumerable<Domain.External.Competition>> GetCompetitions()
        {
            return await _competitionsReader.Read(null);
        }

        public async Task<IEnumerable<Domain.External.Competition>> GetCompetitionsByEventType(string eventTypeId)
        {
            var filter = new MarketFilter()
            { 
                EventTypeIds = new HashSet<string>()
                { 
                    eventTypeId
                }
            };
            return await _competitionsReader.Read(filter);
        }
    }
}

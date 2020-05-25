using BetfairMetadataService.DataAccess.Interfaces.Repositories;
using BetfairMetadataService.DataAccess.Interfaces.WebRequests;
using BetfairMetadataService.Domain.BetfairDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetfairMetadataService.WebRequests.BetfairApi.Repositories
{
    public class BetfairEventsRepository : IExternalEventsRepository
    {
        private IBetfairBatchReader<Domain.External.Event> _eventsReader;

        public BetfairEventsRepository(IBetfairBatchReader<Domain.External.Event> eventsReader)
        {
            _eventsReader = eventsReader;
        }

        public async Task<IEnumerable<Domain.External.Event>> GetEventsByCompetitionId(string competitionId)
        {
            var filter = new MarketFilter()
            { 
                CompetitionIds = new HashSet<string>()
                { 
                    competitionId
                }
            };
            return await _eventsReader.Read(filter);
        }
    }
}

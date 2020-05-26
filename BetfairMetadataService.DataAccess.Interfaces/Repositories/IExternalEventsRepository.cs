using BetfairMetadataService.Domain.External;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetfairMetadataService.DataAccess.Interfaces.Repositories
{
    public interface IExternalEventsRepository
    {
        Task<IEnumerable<Event>> GetEventsByCompetitionIdAndMarketType(string competitionId, string marketType);
    }
}

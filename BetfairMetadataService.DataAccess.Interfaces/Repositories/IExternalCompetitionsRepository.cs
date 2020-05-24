using System.Threading.Tasks;
using System.Collections.Generic;
using BetfairMetadataService.Domain.External;

namespace BetfairMetadataService.DataAccess.Interfaces.Repositories
{
    public interface IExternalCompetitionsRepository
    {
        Task<Competition> GetCompetition(string id);
        Task<IEnumerable<Competition>> GetCompetitions();
        Task<IEnumerable<Competition>> GetCompetitionsByEventType(string eventTypeId);
    }
}

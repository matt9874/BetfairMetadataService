using System.Collections.Generic;
using BetfairMetadataService.Domain.External;
using System.Threading.Tasks;

namespace BetfairMetadataService.DataAccess.Interfaces.Repositories
{
    public interface IExternalEventTypesRepository
    {
        Task<IEnumerable<EventType>> GetEventTypes();
        Task<EventType> GetEventType(string id);
    }
}

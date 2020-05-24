using System.Collections.Generic;
using System.Threading.Tasks;
using BetfairMetadataService.Domain.External;

namespace BetfairMetadataService.DataAccess.Interfaces.Repositories
{
    public interface IExternalMarketTypesRepository
    {
        Task<IEnumerable<MarketType>> GetMarketTypesByCompetitionId(string competitionId);
        Task<IEnumerable<MarketType>> GetMarketTypesByEventTypeId(string eventTypeId);
    }
}

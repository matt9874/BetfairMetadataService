using BetfairMetadataService.Domain.External;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetfairMetadataService.DataAccess.Interfaces
{
    public interface IMarketTypesService
    {
        Task<IEnumerable<MarketType>> GetMarketTypesByEventTypeId(string eventTypeId);
        Task<IEnumerable<MarketType>> GetMarketTypesByCompetitionId(string competitionId);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using BetfairMetadataService.Domain.External;

namespace BetfairMetadataService.DataAccess.Interfaces.Repositories
{
    public interface IExternalMarketsRepository
    {
        Task<Market> GetMarketForEventAndMarketType(string eventId, string marketType);
    }
}

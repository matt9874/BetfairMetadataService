using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.FetchRoots;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BetfairMetadataService.SqlServer.FetchRoots
{
    public class CompetitionMarketTypeFetchRootDeleter : IDeleter<CompetitionMarketType>
    {
        private readonly BetfairMetadataServiceContext _context;

        public CompetitionMarketTypeFetchRootDeleter(BetfairMetadataServiceContext context)
        {
            _context = context;
        }

        public async Task Delete(CompetitionMarketType entity)
        {
            var local = _context.Set<CompetitionMarketType>()
            .Local
            .FirstOrDefault(entry => entry.DataProviderId.Equals(entity.DataProviderId) &&
                entry.CompetitionId.Equals(entity.CompetitionId) &&
                entry.MarketType.Equals(entity.MarketType));

            if (local != null)
            {
                _context.Entry(local).State = EntityState.Deleted;
            }
            else
            {
                _context.CompetitionMarketTypeFetchRoots.Remove(entity);
            }

            await _context.SaveChangesAsync();
        }
    }
}

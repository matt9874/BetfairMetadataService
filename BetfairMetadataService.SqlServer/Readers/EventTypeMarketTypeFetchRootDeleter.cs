using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.FetchRoots;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BetfairMetadataService.SqlServer.Readers
{
    public class EventTypeMarketTypeFetchRootDeleter : IDeleter<EventTypeMarketType>
    {
        private readonly BetfairMetadataServiceContext _context;

        public EventTypeMarketTypeFetchRootDeleter(BetfairMetadataServiceContext context)
        {
            _context = context;
        }

        public async Task Delete(EventTypeMarketType entity)
        {
            var local = _context.Set<EventTypeMarketType>()
            .Local
            .FirstOrDefault(entry => entry.DataProviderId.Equals(entity.DataProviderId) &&
                entry.EventTypeId.Equals(entity.EventTypeId) &&
                entry.MarketType.Equals(entity.MarketType) );

            if (local != null)
            {
                _context.Entry(local).State = EntityState.Deleted;
            }
            else
            {
                _context.EventTypeMarketTypeFetchRoots.Remove(entity);
            }

            await _context.SaveChangesAsync();
        }
    }
}

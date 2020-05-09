using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.FetchRoots;
using System.Threading.Tasks;

namespace BetfairMetadataService.SqlServer.Readers
{
    public class EventTypeMarketTypeFetchRootSaver : ISaver<EventTypeMarketType>
    {
        private readonly BetfairMetadataServiceContext _context;

        public EventTypeMarketTypeFetchRootSaver(BetfairMetadataServiceContext context)
        {
            _context = context;
        }

        public async Task Save(EventTypeMarketType entity)
        {
            await _context.EventTypeMarketTypeFetchRoots.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}

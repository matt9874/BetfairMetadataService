using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.FetchRoots;
using System.Threading.Tasks;

namespace BetfairMetadataService.SqlServer.FetchRoots
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
            _context.EventTypeMarketTypeFetchRoots.Add(entity);
            await _context.SaveChangesAsync();
        }
    }
}

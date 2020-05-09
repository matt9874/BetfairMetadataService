using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.FetchRoots;
using System;
using System.Threading.Tasks;

namespace BetfairMetadataService.SqlServer.Readers
{
    public class EventTypeMarketTypeFetchRootReader : IReader<EventTypeMarketType, Tuple<int, string, string>>
    {
        private readonly BetfairMetadataServiceContext _context;

        public EventTypeMarketTypeFetchRootReader(BetfairMetadataServiceContext context)
        {
            _context = context;
        }

        public async Task<EventTypeMarketType> Read(Tuple<int, string, string> id)
        {
            return await _context.EventTypeMarketTypeFetchRoots.FindAsync(
                    id.Item1, id.Item2, id.Item3);
        }
    }
}

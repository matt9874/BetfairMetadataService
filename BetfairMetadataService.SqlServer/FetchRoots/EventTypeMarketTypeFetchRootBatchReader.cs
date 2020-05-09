using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.FetchRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BetfairMetadataService.SqlServer.FetchRoots
{
    public class EventTypeMarketTypeFetchRootBatchReader : IBatchReader<EventTypeMarketType>
    {
        private readonly BetfairMetadataServiceContext _context;

        public EventTypeMarketTypeFetchRootBatchReader(BetfairMetadataServiceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventTypeMarketType>> Read(Func<EventTypeMarketType,bool> filter)
        {
            return (await _context.EventTypeMarketTypeFetchRoots.ToListAsync())
                .Where(filter);
        }
    }
}

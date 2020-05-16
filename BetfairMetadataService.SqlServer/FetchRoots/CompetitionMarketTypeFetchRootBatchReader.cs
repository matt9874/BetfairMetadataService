using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.FetchRoots;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Linq.Async;
using System.Threading.Tasks;

namespace BetfairMetadataService.SqlServer.FetchRoots
{
    public class CompetitionMarketTypeFetchRootBatchReader : IBatchReader<CompetitionMarketType>
    {
        private readonly BetfairMetadataServiceContext _context;

        public CompetitionMarketTypeFetchRootBatchReader(BetfairMetadataServiceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CompetitionMarketType>> Read(Func<CompetitionMarketType, bool> filter)
        {
            return await _context.CompetitionMarketTypeFetchRoots.AsAsyncEnumerable().Where(filter).ToListAsync();
        }
    }
}

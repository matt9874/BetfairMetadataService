using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetfairMetadataService.SqlServer
{
    public class BatchReader<TEntity> : IBatchReader<TEntity> where TEntity:class
    {
        private readonly BetfairMetadataServiceContext _context;

        public BatchReader(BetfairMetadataServiceContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TEntity>> Read(Func<TEntity, bool> filter)
        {
            return await _context.Set<TEntity>().AsAsyncEnumerable().Where(filter).ToListAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetfairMetadataService.DataAccess.Interfaces
{
    public interface IBatchReader<TEntity>
    {
        Task<IEnumerable<TEntity>> Read(Func<TEntity, bool> filter);
    }
}

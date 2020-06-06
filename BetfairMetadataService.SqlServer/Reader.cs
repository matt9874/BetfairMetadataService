using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.Internal;
using System.Threading.Tasks;

namespace BetfairMetadataService.SqlServer
{
    public class Reader<TEntity> : IReader<TEntity, string> where TEntity : BaseEntity
    {
        private readonly BetfairMetadataServiceContext _context;

        public Reader(BetfairMetadataServiceContext context)
        {
            _context = context;
        }
        public async Task<TEntity> Read(string id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
    }
}

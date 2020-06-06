using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.Internal;
using System.Threading.Tasks;

namespace BetfairMetadataService.SqlServer
{
    public class EntityUpserter : IEntityUpserter
    {
        private readonly BetfairMetadataServiceContext _context;

        public EntityUpserter(BetfairMetadataServiceContext context)
        {
            _context = context;
        }

        public async Task Upsert<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            TEntity savedEntity = await _context.FindAsync<TEntity>(entity.Id);
            if (savedEntity == null)
                _context.Add(entity);
            else
                _context.Entry(savedEntity).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();
        }
    }
}

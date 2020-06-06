using BetfairMetadataService.Domain.Internal;
using System.Threading.Tasks;

namespace BetfairMetadataService.DataAccess.Interfaces
{
    public interface IEntityUpserter
    {
        Task Upsert<TEntity>(TEntity entity) where TEntity : BaseEntity;
    }
}

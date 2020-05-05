using System.Threading.Tasks;

namespace BetfairMetadataService.DataAccess.Interfaces
{
    public interface IReader<TEntity,TId>
    {
        Task<TEntity> Read(TId id);
    }
}

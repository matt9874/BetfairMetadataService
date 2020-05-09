using System.Threading.Tasks;

namespace BetfairMetadataService.DataAccess.Interfaces
{
    public interface IDeleter<TEntity>
    {
        Task Delete(TEntity entity);
    }
}

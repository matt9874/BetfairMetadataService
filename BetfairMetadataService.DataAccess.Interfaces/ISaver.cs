using System.Threading.Tasks;

namespace BetfairMetadataService.DataAccess.Interfaces
{
    public interface ISaver<TEntity>
    {
        Task Save(TEntity entity);
    }
}

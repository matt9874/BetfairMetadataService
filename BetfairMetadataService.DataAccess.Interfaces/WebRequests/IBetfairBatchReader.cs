using System.Collections.Generic;
using System.Threading.Tasks;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.BetfairDtos;

namespace BetfairMetadataService.DataAccess.Interfaces.WebRequests
{
    public interface IBetfairBatchReader<TEntity>
    {
        Task<IEnumerable<TEntity>> Read(BetfairRequestParameters requestParameters);
    }
}

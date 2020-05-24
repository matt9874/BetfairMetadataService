using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.BetfairDtos;
using System.Threading.Tasks;

namespace BetfairMetadataService.DataAccess.Interfaces.WebRequests
{
    public interface IRequestInvokerAsync
    {
        Task<T> Invoke<T>(BetfairMethod method, MarketFilter marketFilter = null);
    }
}

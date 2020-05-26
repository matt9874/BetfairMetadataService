using BetfairMetadataService.Domain;
using System.Threading.Tasks;

namespace BetfairMetadataService.DataAccess.Interfaces.WebRequests
{
    public interface IRequestInvokerAsync
    {
        Task<T> Invoke<T>(BetfairMethod method, BetfairRequestParameters requestParameters);
    }
}

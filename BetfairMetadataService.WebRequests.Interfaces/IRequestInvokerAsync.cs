using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetfairMetadataService.WebRequests.Interfaces
{
    public interface IRequestInvokerAsync
    {
        Task<T> Invoke<T>(string method, IDictionary<string, object> args = null);
    }
}

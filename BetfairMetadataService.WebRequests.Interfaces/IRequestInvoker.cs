using System.Collections.Generic;

namespace BetfairMetadataService.WebRequests.Interfaces
{
    public interface IRequestInvoker
    {
        T Invoke<T>(string method, IDictionary<string, object> args = null);
    }
}

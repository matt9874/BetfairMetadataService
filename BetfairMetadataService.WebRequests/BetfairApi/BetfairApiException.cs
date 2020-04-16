using Newtonsoft.Json.Linq;

namespace BetfairMetadataService.WebRequests.BetfairApi
{
    public class BetfairApiException
    {
        // actual exception details
        public JObject Data { get; set; }

        // exception in rescript format
        public JObject Detail { get; set; }
    }
}

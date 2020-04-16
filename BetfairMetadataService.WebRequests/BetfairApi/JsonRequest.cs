using Newtonsoft.Json;
using System.Collections.Generic;

namespace BetfairMetadataService.WebRequests.BetfairApi
{
    public class JsonRequest
    {
        private const string _jsonRpc = "2.0";

        [JsonProperty(PropertyName = "jsonrpc", NullValueHandling = NullValueHandling.Ignore)]
        public string JsonRpc { get { return _jsonRpc; } }

        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; }

        [JsonProperty(PropertyName = "params")]
        public IDictionary<string, object> Params { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
}

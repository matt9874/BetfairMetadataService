using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BetfairMetadataService.Domain.BetfairDtos
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MarketProjection
    {
        NONE,
        COMPETITION, 
        EVENT, 
        EVENT_TYPE, 
        MARKET_DESCRIPTION, 
        RUNNER_DESCRIPTION, 
        RUNNER_METADATA
    }
}

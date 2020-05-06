using System.Collections.Generic;

namespace BetfairMetadataService.Domain.External
{
    public class EventType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        IEnumerable<MarketType> MarketTypes { get; set; }
    }
}

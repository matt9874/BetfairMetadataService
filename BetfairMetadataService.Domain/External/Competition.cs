using System.Collections.Generic;

namespace BetfairMetadataService.Domain.External
{
    public class Competition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        IEnumerable<MarketType> MarketTypes { get; set; }
    }
}

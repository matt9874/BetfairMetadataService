using BetfairMetadataService.Domain.BetfairDtos;
using System.Collections.Generic;

namespace BetfairMetadataService.Domain
{
    public class BetfairRequestParameters
    {
        public MarketFilter Filter { get; set; }
        public HashSet<MarketProjection> MarketProjections { get; set; }
        public int? MaxResults { get; set; }
    }
}

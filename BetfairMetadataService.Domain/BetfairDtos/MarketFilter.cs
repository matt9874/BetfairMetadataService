using System.Collections.Generic;

namespace BetfairMetadataService.Domain.BetfairDtos
{
    public class MarketFilter
    {
        public string TextQuery { get; set; }
        public ISet<string> ExchangeIds { get; set; }
        public ISet<string> EventTypeIds { get; set; }
        public ISet<string> EventIds { get; set; }
        public ISet<string> CompetitionIds { get; set; }
        public ISet<string> MarketIds { get; set; }
        public ISet<string> Venues { get; set; }
        public bool? BspOnly { get; set; }
        public bool? TurnInPlayEnabled { get; set; }
        public bool? InPlayOnly { get; set; }

        //public ISet<MarketBettingType> MarketBettingTypes { get; set; }

        public ISet<string> MarketCountries { get; set; }

        public ISet<string> MarketTypeCodes { get; set; }

        //public TimeRange MarketStartTime { get; set; }

        //public ISet<OrderStatus> WithOrders { get; set; }
    }
}

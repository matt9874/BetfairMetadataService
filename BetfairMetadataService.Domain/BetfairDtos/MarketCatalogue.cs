using System.Collections.Generic;

namespace BetfairMetadataService.Domain.BetfairDtos
{
    public class MarketCatalogue
    {
        public string MarketId { get; set; }
        public string MarketName { get; set; }
        public bool? MarketDataDelayed { get; set; }
        public MarketDescription Description { get; set; }
        public List<RunnerDescription> Runners { get; set; }
        public EventType EventType { get; set; }
        public Event Event { get; set; }
        public Competition Competition { get; set; }
        public double? TotalMatched { get; set; }
    }
}

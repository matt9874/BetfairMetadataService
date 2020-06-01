using System.Collections.Generic;

namespace BetfairMetadataService.Domain.Internal
{
    public class Selection
    {
        public string Id { get; set; }
        public string MarketId { get; set; }
        public string RunnerName { get; set; }
        public double Handicap { get; set; }
        public int SortPriority { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
    }
}

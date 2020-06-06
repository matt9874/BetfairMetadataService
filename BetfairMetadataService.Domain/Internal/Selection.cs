using System.Collections.Generic;

namespace BetfairMetadataService.Domain.Internal
{
    public class Selection : BaseEntity
    {
        public Market Market { get; set; }
        public string MarketId { get; set; }
        public string RunnerName { get; set; }
        public double Handicap { get; set; }
        public int SortPriority { get; set; }
    }
}

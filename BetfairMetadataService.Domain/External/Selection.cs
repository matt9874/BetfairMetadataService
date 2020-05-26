using System.Collections.Generic;

namespace BetfairMetadataService.Domain.External
{
    public class Selection
    {
        public long SelectionId { get; set; }
        public string RunnerName { get; set; }
        public double Handicap { get; set; }
        public int SortPriority { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
    }
}

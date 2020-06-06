namespace BetfairMetadataService.API.Models.Internal
{
    public class SelectionDto
    {
        public string Id { get; set; }
        public string MarketId { get; set; }
        public string RunnerName { get; set; }
        public double Handicap { get; set; }
        public int SortPriority { get; set; }
    }
}

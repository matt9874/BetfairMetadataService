namespace BetfairMetadataService.API.Models.Internal
{
    public class CompetitionDto: LinkResourceBaseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string EventTypeId { get; set; }
    }
}

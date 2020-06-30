using System;

namespace BetfairMetadataService.API.Models.Internal
{
    public class EventDto: LinkResourceBaseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string Timezone { get; set; }
        public string Venue { get; set; }
        public DateTime? OpenDate { get; set; }
        public string CompetitionId { get; set; }
    }
}

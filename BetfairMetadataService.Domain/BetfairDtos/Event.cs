using System;

namespace BetfairMetadataService.Domain.BetfairDtos
{
    public class Event
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string Timezone { get; set; }
        public string Venue { get; set; }
        public DateTime? OpenDate { get; set; }
    }
}

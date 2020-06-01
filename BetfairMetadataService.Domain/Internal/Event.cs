using System;
using System.Collections.Generic;

namespace BetfairMetadataService.Domain.Internal
{
    public class Event
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string Timezone { get; set; }
        public string Venue { get; set; }
        public DateTime? OpenDate { get; set; }
        ICollection<Market> Markets { get; set; }
        public string CompetitionId { get; set; }
    }
}

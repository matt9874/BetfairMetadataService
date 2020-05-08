using System.Collections.Generic;

namespace BetfairMetadataService.Domain.Internal
{
    public class Competition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        ICollection<Event> Events { get; set; }
    }
}

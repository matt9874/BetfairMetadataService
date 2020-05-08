using System.Collections.Generic;

namespace BetfairMetadataService.Domain.Internal
{
    public class EventType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        ICollection<Competition> Competitions { get; set; }
        ICollection<Event> Events { get; set; }
    }
}

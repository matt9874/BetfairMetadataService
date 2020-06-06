using System.Collections.Generic;

namespace BetfairMetadataService.Domain.Internal
{
    public class Competition : BaseEntity
    {
        public string Name { get; set; }
        public string Region { get; set; }
        ICollection<Event> Events { get; set; }
        public string EventTypeId { get; set; }
    }
}

using System.Collections.Generic;

namespace BetfairMetadataService.Domain.Internal
{
    public class EventType : BaseEntity
    {
        public string Name { get; set; }
        ICollection<Competition> Competitions { get; set; }
    }
}

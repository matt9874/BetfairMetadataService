using System.Text;

namespace BetfairMetadataService.Domain.BetfairDtos
{
    public class EventType
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}", "EventType")
                        .AppendFormat(" : Id={0}", Id)
                        .AppendFormat(" : Name={0}", Name)
                        .ToString();
        }
    }
}

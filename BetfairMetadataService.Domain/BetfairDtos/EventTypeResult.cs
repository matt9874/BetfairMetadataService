using System.Text;

namespace BetfairMetadataService.Domain.BetfairDtos
{
    public class EventTypeResult
    {
        public EventType EventType { get; set; }
        public int MarketCount { get; set; }

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}", "EventTypeResult")
                        .AppendFormat(" : {0}", EventType)
                        .AppendFormat(" : MarketCount={0}", MarketCount)
                        .ToString();
        }
    }
}

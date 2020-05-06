using System.Text;

namespace BetfairMetadataService.Domain.BetfairDtos
{
    public class CompetitionResult
    {
        public Competition Competition { get; set; }
        public int MarketCount { get; set; }
        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}", "CompetitionResult")
                        .AppendFormat(" : {0}", Competition)
                        .AppendFormat(" : MarketCount={0}", MarketCount)
                        .ToString();
        }
    }
}

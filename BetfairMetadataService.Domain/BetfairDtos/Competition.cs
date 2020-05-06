using System.Text;

namespace BetfairMetadataService.Domain.BetfairDtos
{
    public class Competition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}", "Competition")
                        .AppendFormat(" : Id={0}", Id)
                        .AppendFormat(" : Name={0}", Name)
                        .ToString();
        }
    }
}

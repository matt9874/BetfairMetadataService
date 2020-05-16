using System.ComponentModel.DataAnnotations;

namespace BetfairMetadataService.API.Models.FetchRoots
{
    public class CompetitionMarketTypeCreationDto
    {
        [Required]
        public string MarketType { get; set; }
    }
}

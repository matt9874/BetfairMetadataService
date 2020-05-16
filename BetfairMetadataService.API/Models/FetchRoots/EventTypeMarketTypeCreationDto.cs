using System.ComponentModel.DataAnnotations;

namespace BetfairMetadataService.API.Models.FetchRoots
{
    public class EventTypeMarketTypeCreationDto
    {

        [Required]
        public string MarketType { get; set; }
    }
}

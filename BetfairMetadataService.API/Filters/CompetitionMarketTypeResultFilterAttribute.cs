using AutoMapper;
using BetfairMetadataService.API.Models.FetchRoots;

namespace BetfairMetadataService.API.Filters
{
    public class CompetitionMarketTypeResultFilterAttribute : MappingResultFilterAttribute
    {
        protected override object MapActionResultValue(IMapper mapper, object value)
        {
            return mapper.Map<CompetitionMarketTypeDto>(value);
        }
    }
}

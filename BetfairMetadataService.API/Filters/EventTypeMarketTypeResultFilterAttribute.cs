using AutoMapper;
using BetfairMetadataService.API.Models.FetchRoots;

namespace BetfairMetadataService.API.Filters
{
    public class EventTypeMarketTypeResultFilterAttribute : MappingResultFilterAttribute
    {
        protected override object MapActionResultValue(IMapper mapper, object value)
        {
            return mapper.Map<EventTypeMarketTypeDto>(value);
        }
    }
}

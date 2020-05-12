using AutoMapper;
using BetfairMetadataService.API.Models.FetchRoots;
using System.Collections.Generic;

namespace BetfairMetadataService.API.Filters
{
    public class EventTypeMarketTypesResultFilterAttribute : MappingResultFilterAttribute
    {
        protected override object MapActionResultValue(IMapper mapper, object value)
        {
            return mapper.Map<IEnumerable<EventTypeMarketTypeDto>>(value);
        }
    }
}

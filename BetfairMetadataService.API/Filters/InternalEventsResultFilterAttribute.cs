using AutoMapper;
using BetfairMetadataService.API.Models.Internal;
using System.Collections.Generic;

namespace BetfairMetadataService.API.Filters
{
    public class InternalEventsResultFilterAttribute : MappingResultFilterAttribute
    {
        protected override object MapActionResultValue(IMapper mapper, object value)
        {
            return mapper.Map<IEnumerable<EventDto>>(value);
        }
    }
}

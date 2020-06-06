using AutoMapper;
using BetfairMetadataService.API.Models.Internal;
using System.Collections.Generic;

namespace BetfairMetadataService.API.Filters
{
    public class InternalSelectionsResultFilterAttribute : MappingResultFilterAttribute
    {
        protected override object MapActionResultValue(IMapper mapper, object value)
        {
            return mapper.Map<IEnumerable<SelectionDto>>(value);
        }
    }
}

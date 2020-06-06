using AutoMapper;
using BetfairMetadataService.API.Models.Internal;
using System.Collections.Generic;

namespace BetfairMetadataService.API.Filters
{
    public class InternalCompetitionsResultFilterAttribute : MappingResultFilterAttribute
    {
        protected override object MapActionResultValue(IMapper mapper, object value)
        {
            return mapper.Map<IEnumerable<CompetitionDto>>(value);
        }
    }
}

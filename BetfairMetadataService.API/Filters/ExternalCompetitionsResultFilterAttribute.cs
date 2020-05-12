using AutoMapper;
using BetfairMetadataService.API.Models.External;
using System.Collections.Generic;

namespace BetfairMetadataService.API.Filters
{
    public class ExternalCompetitionsResultFilterAttribute : MappingResultFilterAttribute
    {
        protected override object MapActionResultValue(IMapper mapper, object value)
        {
            return mapper.Map<IEnumerable<CompetitionDto>>(value);
        }
    }
}

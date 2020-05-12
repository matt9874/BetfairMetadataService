using AutoMapper;
using BetfairMetadataService.API.Models.External;

namespace BetfairMetadataService.API.Filters
{
    public class ExternalDataProviderResultFilterAttribute: MappingResultFilterAttribute
    {
        protected override object MapActionResultValue(IMapper mapper, object value)
        {
            return mapper.Map<DataProviderDto>(value);
        }
    }
}

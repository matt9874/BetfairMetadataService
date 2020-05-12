using AutoMapper;
using BetfairMetadataService.API.Models.External;
using System.Collections.Generic;

namespace BetfairMetadataService.API.Filters
{
    public class ExternalMarketTypesResultFilterAttribute : MappingResultFilterAttribute
    {
        protected override object MapActionResultValue(IMapper mapper, object value)
        {
            return mapper.Map<IEnumerable<MarketTypeDto>>(value);
        }
    }
}

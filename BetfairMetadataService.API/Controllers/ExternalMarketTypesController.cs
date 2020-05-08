using AutoMapper;
using BetfairMetadataService.API.Models.External;
using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.External;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetfairMetadataService.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ExternalMarketTypesController : ControllerBase
    {
        private readonly Func<int, IMarketTypesService> _marketTypesServiceFactory;
        private readonly IMapper _mapper;

        public ExternalMarketTypesController(Func<int, IMarketTypesService> marketTypesServiceFactory, IMapper mapper)
        {
            _marketTypesServiceFactory = marketTypesServiceFactory;
            _mapper = mapper;
        }

        [HttpGet("dataProviders/{dataProviderId}/competitions/{competitionId}/marketTypes")]
        public async Task<IActionResult> GetMarketTypesByCompetition(int dataProviderId, string competitionId)
        {
            IMarketTypesService marketTypesService = _marketTypesServiceFactory?.Invoke(dataProviderId);
            IEnumerable<MarketType> marketTypes = await marketTypesService.GetMarketTypesByCompetitionId(competitionId);

            if (marketTypes == null)
                throw new Exception($"IMarketTypesService.GetMarketTypesByCompetitionId({competitionId}) returned null IEnumerable<MarketType>");

            IList<MarketTypeDto> marketTypeDtos = _mapper.Map<IList<MarketTypeDto>>(marketTypes);
            return Ok(marketTypeDtos);
        }

        [HttpGet("dataProviders/{dataProviderId}/eventTypes/{eventTypeId}/marketTypes")]
        public async Task<IActionResult> GetMarketTypesByEventType(int dataProviderId, string eventTypeId)
        {
            IMarketTypesService marketTypesService = _marketTypesServiceFactory?.Invoke(dataProviderId);
            IEnumerable<MarketType> marketTypes = await marketTypesService.GetMarketTypesByEventTypeId(eventTypeId);

            if (marketTypes == null)
                throw new Exception($"IMarketTypesService.GetMarketTypesByEventTypeId({eventTypeId}) returned null IEnumerable<MarketType>");

            IList<MarketTypeDto> marketTypeDtos = _mapper.Map<IList<MarketTypeDto>>(marketTypes);
            return Ok(marketTypeDtos);
        }
    }
}

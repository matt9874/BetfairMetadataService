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
        private readonly IReader<DataProvider, int> _dataProviderReader;
        private readonly IReader<EventType, string> _eventTypeReader;
        private readonly IReader<Competition, string> _competitionReader;
        private readonly Func<int, IMarketTypesService> _marketTypesServiceFactory;
        private readonly IMapper _mapper;

        public ExternalMarketTypesController(IReader<DataProvider, int> dataProviderReader, IReader<EventType, string> eventTypeReader,
            IReader<Competition, string> competitionReader, Func<int, IMarketTypesService> marketTypesServiceFactory, IMapper mapper)
        {
            _dataProviderReader = dataProviderReader;
            _eventTypeReader = eventTypeReader;
            _competitionReader = competitionReader;
            _marketTypesServiceFactory = marketTypesServiceFactory;
            _mapper = mapper;
        }

        [HttpGet("dataProviders/{dataProviderId}/competitions/{competitionId}/marketTypes")]
        public async Task<IActionResult> GetMarketTypesByCompetition(int dataProviderId, string competitionId)
        {
            DataProvider dataProvider = await _dataProviderReader.Read(dataProviderId);
            if (dataProvider == null)
                return NotFound($"Unable to find dataProvider with id {dataProviderId}");

            Competition competition = await _competitionReader.Read(competitionId);
            if (competition == null)
                return NotFound($"Unable to find competition with id {competitionId}");

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
            DataProvider dataProvider = await _dataProviderReader.Read(dataProviderId);
            if (dataProvider == null)
                return NotFound($"Unable to find dataProvider with id {dataProviderId}");

            EventType eventType = await _eventTypeReader.Read(eventTypeId);
            if (eventType == null)
                return NotFound($"Unable to find eventType with id {eventTypeId}");

            IMarketTypesService marketTypesService = _marketTypesServiceFactory?.Invoke(dataProviderId);
            IEnumerable<MarketType> marketTypes = await marketTypesService.GetMarketTypesByEventTypeId(eventTypeId);

            if (marketTypes == null)
                throw new Exception($"IMarketTypesService.GetMarketTypesByEventTypeId({eventTypeId}) returned null IEnumerable<MarketType>");

            IList<MarketTypeDto> marketTypeDtos = _mapper.Map<IList<MarketTypeDto>>(marketTypes);
            return Ok(marketTypeDtos);
        }
    }
}

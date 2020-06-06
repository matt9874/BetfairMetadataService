using BetfairMetadataService.API.Filters;
using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.DataAccess.Interfaces.Repositories;
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
        private readonly Func<int, IExternalEventTypesRepository> _eventTypesRepositoryFactory;
        private readonly Func<int, IExternalCompetitionsRepository> _competitionsRepositoryFactory;
        private readonly Func<int, IExternalMarketTypesRepository> _marketTypesRepositoryFactory;

        public ExternalMarketTypesController(IReader<DataProvider, int> dataProviderReader, 
            Func<int, IExternalEventTypesRepository> eventTypesRepositoryFactory,
            Func<int, IExternalCompetitionsRepository> competitionsRepositoryFactory,
            Func<int, IExternalMarketTypesRepository> marketTypesrepositoryFactory)
        {
            _dataProviderReader = dataProviderReader;
            _eventTypesRepositoryFactory = eventTypesRepositoryFactory;
            _competitionsRepositoryFactory = competitionsRepositoryFactory;
            _marketTypesRepositoryFactory = marketTypesrepositoryFactory;
        }

        [HttpGet("dataProviders/{dataProviderId}/eventTypes/{eventTypeId}/competitions/{competitionId}/marketTypes")]
        [ThrowOnNullCollectionResultFilter]
        [ExternalMarketTypesResultFilterAttribute]
        public async Task<IActionResult> GetMarketTypesByCompetition(int dataProviderId, string eventTypeId, string competitionId)
        {
            DataProvider dataProvider = await _dataProviderReader.Read(dataProviderId);
            if (dataProvider == null)
                return NotFound($"Unable to find dataProvider with id {dataProviderId}");

            IExternalEventTypesRepository eventTypesRepository = _eventTypesRepositoryFactory?.Invoke(dataProviderId);
            EventType eventType = await eventTypesRepository.GetEventType(eventTypeId);
            if (eventType == null)
                return NotFound($"Unable to find event type with id {eventTypeId}");

            IExternalCompetitionsRepository competitionsRepository = _competitionsRepositoryFactory?.Invoke(dataProviderId);
            Competition competition = await competitionsRepository.GetCompetition(competitionId);
            if (competition == null)
                return NotFound($"Unable to find competition with id {competitionId}");

            IExternalMarketTypesRepository marketTypesRepository = _marketTypesRepositoryFactory?.Invoke(dataProviderId);
            IEnumerable<MarketType> marketTypes = await marketTypesRepository.GetMarketTypesByCompetitionId(competitionId);

            return Ok(marketTypes);
        }

        [HttpGet("dataProviders/{dataProviderId}/eventTypes/{eventTypeId}/marketTypes")]
        [ThrowOnNullCollectionResultFilter]
        [ExternalMarketTypesResultFilter]
        public async Task<IActionResult> GetMarketTypesByEventType(int dataProviderId, string eventTypeId)
        {
            DataProvider dataProvider = await _dataProviderReader.Read(dataProviderId);
            if (dataProvider == null)
                return NotFound($"Unable to find dataProvider with id {dataProviderId}");

            IExternalEventTypesRepository eventTypesRepository = _eventTypesRepositoryFactory?.Invoke(dataProviderId);
            EventType eventType = await eventTypesRepository.GetEventType(eventTypeId);
            if (eventType == null)
                return NotFound($"Unable to find event type with id {eventTypeId}");

            IExternalMarketTypesRepository marketTypesRepository = _marketTypesRepositoryFactory?.Invoke(dataProviderId);
            IEnumerable<MarketType> marketTypes = await marketTypesRepository.GetMarketTypesByEventTypeId(eventTypeId);

            return Ok(marketTypes);
        }

        [HttpOptions("marketTypes")]
        public Task<IActionResult> GetMarketTypesOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS");
            IActionResult actionResult = Ok();
            return Task.FromResult(actionResult);
        }
    }
}

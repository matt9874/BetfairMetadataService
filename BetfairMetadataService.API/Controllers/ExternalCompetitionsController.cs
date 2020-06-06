using BetfairMetadataService.API.Filters;
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
    public class ExternalCompetitionsController : ControllerBase
    {
        private readonly Func<int, IExternalCompetitionsRepository> _competitionsRepositoryFactory;
        private readonly Func<int, IExternalEventTypesRepository> _eventTypesRepositoryFactory;

        public ExternalCompetitionsController(Func<int, IExternalCompetitionsRepository> competitionsRepositoryFactory,
            Func<int, IExternalEventTypesRepository> eventTypesRepositoryFactory)
        {
            _competitionsRepositoryFactory = competitionsRepositoryFactory;
            _eventTypesRepositoryFactory = eventTypesRepositoryFactory;
        }

        [HttpGet("dataProviders/{dataProviderId}/competitions")]
        [ThrowOnNullCollectionResultFilter]
        [ExternalCompetitionsResultFilter]
        public async Task<IActionResult> GetCompetitions(int dataProviderId)
        {
            IExternalCompetitionsRepository repository = _competitionsRepositoryFactory?.Invoke(dataProviderId);
            IEnumerable<Competition> competitions = await repository.GetCompetitions();

            return Ok(competitions);
        }

        [HttpGet("dataProviders/{dataProviderId}/eventTypes/{eventTypeId}/competitions")]
        [ThrowOnNullCollectionResultFilter]
        [ExternalCompetitionsResultFilter]
        public async Task<IActionResult> GetCompetitionsByEventType(int dataProviderId, string eventTypeId)
        {
            IExternalEventTypesRepository eventTypesRepository = _eventTypesRepositoryFactory?.Invoke(dataProviderId);
            if (eventTypesRepository == null)
                return NotFound($"Unable to find data provider with id of {dataProviderId}");

            EventType eventType = await eventTypesRepository.GetEventType(eventTypeId);
            if (eventType == null)
                return NotFound($"Could not find event type with id of {eventTypeId} for data provider with id {dataProviderId}");

            IExternalCompetitionsRepository competitionsRepository = _competitionsRepositoryFactory?.Invoke(dataProviderId);
            IEnumerable<Competition> competitions = await competitionsRepository.GetCompetitionsByEventType(eventTypeId);
            
            return Ok(competitions);
        }

        [HttpOptions("competitions")]
        public Task<IActionResult> GetCompetitionsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS");
            IActionResult actionResult = Ok();
            return Task.FromResult(actionResult);
        }
    }
}

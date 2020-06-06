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
    public class ExternalEventTypesController: ControllerBase
    {
        private readonly Func<int, IExternalEventTypesRepository> _repositoryFactory;

        public ExternalEventTypesController(Func<int, IExternalEventTypesRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        [HttpGet("dataProviders/{dataProviderId}/eventTypes")]
        [ExternalEventTypesResultFilter]
        public async Task<IActionResult> GetEventTypes(int dataProviderId)
        {
            IExternalEventTypesRepository repository = _repositoryFactory?.Invoke(dataProviderId);
            if (repository == null)
                return NotFound($"Unable to find data provider with id of {dataProviderId}");

            IEnumerable<EventType> eventTypes = await repository.GetEventTypes();
            if (eventTypes == null)
                throw new Exception("Repository returned null IEnumerable");

            return Ok(eventTypes);
        }

        [HttpOptions("eventTypes")]
        public Task<IActionResult> GetEventTypesOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS");
            IActionResult actionResult = Ok();
            return Task.FromResult(actionResult);
        }
    }
}

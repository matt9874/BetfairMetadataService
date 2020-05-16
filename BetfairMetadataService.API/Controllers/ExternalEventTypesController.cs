using BetfairMetadataService.API.Filters;
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
    public class ExternalEventTypesController: ControllerBase
    {
        private readonly Func<int, IBatchReader<EventType>> _batchReaderFactory;

        public ExternalEventTypesController(Func<int, IBatchReader<EventType>> batchReaderFactory)
        {
            _batchReaderFactory = batchReaderFactory;
        }

        [HttpGet("dataProviders/{dataProviderId}/eventTypes")]
        [ExternalEventTypesResultFilter]
        public async Task<IActionResult> GetEventTypes(int dataProviderId)
        {
            IBatchReader<EventType> reader = _batchReaderFactory?.Invoke(dataProviderId);
            IEnumerable<EventType> eventTypes = await reader.Read(et=>true);
            if (eventTypes == null)
                throw new Exception("IBatchReader<EventType> returned null IEnumerable");

            return Ok(eventTypes);
        }

        [HttpOptions]
        public Task<IActionResult> GetEventTypesOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS");
            IActionResult actionResult = Ok();
            return Task.FromResult(actionResult);
        }
    }
}

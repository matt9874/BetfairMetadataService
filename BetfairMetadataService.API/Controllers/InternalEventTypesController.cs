using BetfairMetadataService.API.Filters;
using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.Internal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetfairMetadataService.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class InternalEventTypesController : ControllerBase
    {
        private readonly IBatchReader<EventType> _batchReader;

        public InternalEventTypesController(IBatchReader<EventType> batchReader)
        {
            _batchReader = batchReader;
        }

        [HttpGet("eventTypes")]
        [ThrowOnNullCollectionResultFilter]
        [InternalEventTypesResultFilter]
        public async Task<IActionResult> GetEventTypes()
        {
            IEnumerable<EventType> eventTypes = await _batchReader.Read(et => true);
            return Ok(eventTypes);
        }
    }
}

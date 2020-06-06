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
    public class InternalMarketsController : ControllerBase
    {
        private readonly IBatchReader<Market> _batchReader;
        private readonly IReader<Event, string> _eventReader;

        public InternalMarketsController(IBatchReader<Market> batchReader, IReader<Event, string> eventReader)
        {
            _batchReader = batchReader;
            _eventReader = eventReader;
        }

        [HttpGet("markets")]
        [InternalMarketsResultFilter]
        public async Task<IActionResult> GetMarkets()
        {
            IEnumerable<Market> markets = await _batchReader.Read(e => true);
            if (markets == null)
                throw new Exception("IBatchReader<Market> returned null IEnumerable");

            return Ok(markets);
        }

        [HttpGet("events/{eventId}/markets")]
        [InternalMarketsResultFilter]
        public async Task<IActionResult> GetMarketsByEventId(string eventId)
        {
            Event evt = await _eventReader.Read(eventId);
            if (evt == null)
                return NotFound($"Could not find Event with id of {eventId}");

            IEnumerable<Market> markets = await _batchReader.Read(e => e.EventId == eventId);
            if (markets == null)
                throw new Exception("IBatchReader<Market> returned null IEnumerable");

            return Ok(markets);
        }
    }
}

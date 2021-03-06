﻿using BetfairMetadataService.API.Filters;
using BetfairMetadataService.API.Filters.LinkAddingResultFilters;
using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetfairMetadataService.API.Controllers
{
    [Route("api/")]
    [ApiController]
    [ResponseCache(CacheProfileName = "240SecondsCacheProfile")]
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
        [ThrowOnNullCollectionResultFilter]
        [InternalMarketsResultFilter]
        public async Task<IActionResult> GetMarkets()
        {
            IEnumerable<Market> markets = await _batchReader.Read(e => true);
            return Ok(markets);
        }

        [HttpGet("events/{eventId}/markets", Name = "GetMarketsByEvent")]
        [ThrowOnNullCollectionResultFilter]
        [InternalMarketsResultFilter]
        [MarketsLinksResultFilter]
        public async Task<IActionResult> GetMarketsByEventId(string eventId)
        {
            Event evt = await _eventReader.Read(eventId);
            if (evt == null)
                return NotFound($"Could not find Event with id of {eventId}");

            IEnumerable<Market> markets = await _batchReader.Read(e => e.EventId == eventId);
            return Ok(markets);
        }
    }
}

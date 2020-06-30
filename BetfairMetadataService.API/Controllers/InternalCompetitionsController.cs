using BetfairMetadataService.API.Filters;
using BetfairMetadataService.API.Filters.LinkAddingResultFilters;
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
    public class InternalCompetitionsController : ControllerBase
    {
        private readonly IBatchReader<Competition> _batchReader;
        private readonly IReader<EventType, string> _eventTypeReader;

        public InternalCompetitionsController(IBatchReader<Competition> batchReader, IReader<EventType, string> eventTypeReader)
        {
            _batchReader = batchReader;
            _eventTypeReader = eventTypeReader;
        }

        [HttpGet("competitions")]
        [ThrowOnNullCollectionResultFilter]
        [InternalCompetitionsResultFilter]
        public async Task<IActionResult> GetCompetitions()
        {
            IEnumerable<Competition> competitions = await _batchReader.Read(et => true);
            return Ok(competitions);
        }

        [HttpGet("eventTypes/{eventTypeId}/competitions", Name = "GetCompetitionsByEventType")]
        [ThrowOnNullCollectionResultFilter]
        [InternalCompetitionsResultFilter]
        [CompetitionsLinksResultFilter]
        public async Task<IActionResult> GetCompetitionsByEventTypeId(string eventTypeId)
        {
            EventType eventType = await _eventTypeReader.Read(eventTypeId);
            if (eventType == null)
                return NotFound($"Could not find EventType with id of {eventTypeId}");

            IEnumerable<Competition> competitions = await _batchReader.Read(c => c.EventTypeId == eventTypeId);
            return Ok(competitions);
        }
    }
}

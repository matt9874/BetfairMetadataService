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
    public class InternalEventsController : ControllerBase
    {
        private readonly IBatchReader<Event> _batchReader;
        private readonly IReader<Competition, string> _competitionReader;

        public InternalEventsController(IBatchReader<Event> batchReader, IReader<Competition, string> competitionReader)
        {
            _batchReader = batchReader;
            _competitionReader = competitionReader;
        }

        [HttpGet("events")]
        [ThrowOnNullCollectionResultFilter]
        [InternalEventsResultFilter]
        public async Task<IActionResult> GetEvents()
        {
            IEnumerable<Event> events = await _batchReader.Read(e => true);
            return Ok(events);
        }

        [HttpGet("competitions/{competitionId}/events")]
        [ThrowOnNullCollectionResultFilter]
        [InternalEventsResultFilter]
        public async Task<IActionResult> GetEventsByCompetitionId(string competitionId)
        {
            Competition competition = await _competitionReader.Read(competitionId);
            if (competition == null)
                return NotFound($"Could not find Competition with id of {competitionId}");

            IEnumerable<Event> events = await _batchReader.Read(e => e.CompetitionId == competitionId);
            return Ok(events);
        }
    }
}

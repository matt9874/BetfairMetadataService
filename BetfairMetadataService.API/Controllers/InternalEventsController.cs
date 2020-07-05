using BetfairMetadataService.API.Filters;
using BetfairMetadataService.API.ResourceParameters.Internal;
using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.Internal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BetfairMetadataService.Domain.Common.Extensions;
using BetfairMetadataService.API.Filters.LinkAddingResultFilters;

namespace BetfairMetadataService.API.Controllers
{
    [Route("api/")]
    [ApiController]
    [ResponseCache(CacheProfileName = "240SecondsCacheProfile")]
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
        public async Task<IActionResult> GetEvents([FromQuery] EventResourceParameters eventResourceParameters)
        {
            Func<Event, bool> filter = GetFilter(eventResourceParameters);

            IEnumerable<Event> events = await _batchReader.Read(filter);
            return Ok(events);
        }

        [HttpGet("competitions/{competitionId}/events", Name = "GetEventsByCompetition")]
        [ThrowOnNullCollectionResultFilter]
        [InternalEventsResultFilter]
        [EventsLinksResultFilter]
        public async Task<IActionResult> GetEventsByCompetitionId(string competitionId, [FromQuery] EventResourceParameters eventResourceParameters)
        {
            Competition competition = await _competitionReader.Read(competitionId);
            if (competition == null)
                return NotFound($"Could not find Competition with id of {competitionId}");

            Func<Event, bool> filter = GetFilter(eventResourceParameters);
            IEnumerable<Event> events = await _batchReader.Read(e => filter(e) && e.CompetitionId == competitionId);
            return Ok(events);
        }

        private static Func<Event, bool> GetFilter(EventResourceParameters eventResourceParameters)
        {
            Expression<Func<Event, bool>> filterExpression = e => true;
            if (eventResourceParameters.FromDate != null)
            {
                Expression<Func<Event, bool>> fromFilterExpression = e => e.OpenDate >= eventResourceParameters.FromDate;
                filterExpression = filterExpression.And(fromFilterExpression);
            }
            if (eventResourceParameters.ToDate != null)
            {
                Expression<Func<Event, bool>> toFilterExpression = e => e.OpenDate <= eventResourceParameters.ToDate;
                filterExpression = filterExpression.And(toFilterExpression);
            }
            return filterExpression.Compile();

        }
    }
}

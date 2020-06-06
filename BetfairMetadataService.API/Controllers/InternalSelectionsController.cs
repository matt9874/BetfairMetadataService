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
    public class InternalSelectionsController : ControllerBase
    {
        private readonly IBatchReader<Selection> _batchReader;
        private readonly IReader<Market, string> _marketReader;

        public InternalSelectionsController(IBatchReader<Selection> batchReader, IReader<Market, string> competitionReader)
        {
            _batchReader = batchReader;
            _marketReader = competitionReader;
        }

        [HttpGet("selections")]
        [ThrowOnNullCollectionResultFilter]
        [InternalSelectionsResultFilter]
        public async Task<IActionResult> GetSelections()
        {
            IEnumerable<Selection> selections = await _batchReader.Read(s => true);
            return Ok(selections);
        }

        [HttpGet("markets/{marketId}/selections")]
        [ThrowOnNullCollectionResultFilter]
        [InternalSelectionsResultFilter]
        public async Task<IActionResult> GetSelectionsByMarketId(string marketId)
        {
            Market market = await _marketReader.Read(marketId);
            if (market == null)
                return NotFound($"Could not find Market with id of {marketId}");

            IEnumerable<Selection> selections = await _batchReader.Read(e => e.MarketId == marketId);
            return Ok(selections);
        }
    }
}

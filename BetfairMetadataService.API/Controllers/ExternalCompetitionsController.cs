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
    public class ExternalCompetitionsController : ControllerBase
    {
        private readonly Func<int, IBatchReader<Competition>> _batchReaderFactory;

        public ExternalCompetitionsController(Func<int, IBatchReader<Competition>> batchReaderFactory)
        {
            _batchReaderFactory = batchReaderFactory;
        }

        [HttpGet("dataProviders/{dataProviderId}/competitions")]
        [ExternalCompetitionsResultFilterAttribute]
        public async Task<IActionResult> GetCompetitions(int dataProviderId)
        {
            IBatchReader<Competition> reader = _batchReaderFactory?.Invoke(dataProviderId);
            IEnumerable<Competition> competitions = await reader.Read(c => true);
            if (competitions == null)
                throw new Exception("IBatchReader<Competition> returned null IEnumerable");

            return Ok(competitions);
        }

        [HttpOptions]
        public Task<IActionResult> GetCompetitionsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS");
            IActionResult actionResult = Ok();
            return Task.FromResult(actionResult);
        }
    }
}

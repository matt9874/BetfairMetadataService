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
    public class ExternalDataProvidersController: ControllerBase
    {
        private readonly IReader<DataProvider, int> _dataProviderReader;
        private readonly IBatchReader<DataProvider> _batchDataProviderReader;

        public ExternalDataProvidersController(IReader<DataProvider, int> dataProviderReader, 
            IBatchReader<DataProvider> batchDataProviderReader)
        {
            _dataProviderReader = dataProviderReader;
            _batchDataProviderReader = batchDataProviderReader;
        }

        [HttpGet("dataProviders")]
        [ExternalDataProvidersResultFilter]
        public async Task<IActionResult> GetDataProviders()
        {
            IEnumerable<DataProvider> dataProviders = await _batchDataProviderReader.Read(dp => true);
            if (dataProviders == null)
                throw new Exception("IBatchReader<DataProvider> returned null");

            return Ok(dataProviders);
        }

        [HttpGet("dataProviders/{dataProviderId}")]
        [ExternalDataProviderResultFilter]
        public async Task<IActionResult> GetDataProvider(int dataProviderId)
        {
            DataProvider dataProvider = await _dataProviderReader.Read(dataProviderId);

            if (dataProvider == null)
                return NotFound(dataProviderId);

            return Ok(dataProvider);
        }

        [HttpOptions]
        public Task<IActionResult> GetDataProvidersOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS");
            IActionResult actionResult = Ok();
            return Task.FromResult(actionResult);
        }
    }
}

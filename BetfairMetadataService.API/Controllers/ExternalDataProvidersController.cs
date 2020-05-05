using AutoMapper;
using BetfairMetadataService.API.Models.External;
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
        private readonly IMapper _mapper;
        private readonly IReader<DataProvider, int> _dataProviderReader;
        private readonly IBatchReader<DataProvider> _batchDataProviderReader;

        public ExternalDataProvidersController(IMapper mapper, IReader<DataProvider, int> dataProviderReader, 
            IBatchReader<DataProvider> batchDataProviderReader)
        {
            _mapper = mapper;
            _dataProviderReader = dataProviderReader;
            _batchDataProviderReader = batchDataProviderReader;
        }

        [HttpGet("dataProviders")]
        public async Task<IActionResult> GetDataProviders()
        {
            IEnumerable<DataProvider> dataProviders = await _batchDataProviderReader.Read(dp => true);
            if (dataProviders == null)
                throw new Exception("IBatchReader<DataProvider> returned null");

            IEnumerable<DataProviderDto> dataProviderDtos = _mapper.Map<IEnumerable<DataProviderDto>>(dataProviders);
            return Ok(dataProviderDtos);
        }

        [HttpGet("dataProviders/{dataProviderId}")]
        public async Task<IActionResult> GetDataProvider(int dataProviderId)
        {
            DataProvider dataProvider = await _dataProviderReader.Read(dataProviderId);

            if (dataProvider == null)
                return NotFound(dataProviderId);

            DataProviderDto dataProviderDto = _mapper.Map<DataProviderDto>(dataProvider);
            return Ok(dataProviderDto);
        }
    }
}

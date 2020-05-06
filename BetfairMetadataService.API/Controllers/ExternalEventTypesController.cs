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
    public class ExternalEventTypesController: ControllerBase
    {
        private readonly Func<int, IBatchReader<EventType>> _batchReaderFactory;
        private readonly IMapper _mapper;

        public ExternalEventTypesController(Func<int, IBatchReader<EventType>> batchReaderFactory, IMapper mapper)
        {
            _batchReaderFactory = batchReaderFactory;
            _mapper = mapper;
        }

        [HttpGet("dataProviders/{dataProviderId}/eventTypes")]
        public async Task<IActionResult> GetEventTypes(int dataProviderId)
        {
            IBatchReader<EventType> reader = _batchReaderFactory?.Invoke(dataProviderId);
            IEnumerable<EventType> eventTypes = await reader.Read(et=>true);
            if (eventTypes == null)
                throw new Exception("IBatchReader<EventType> returned null IEnumerable");

            IEnumerable<EventTypeDto> eventTypeDtos = _mapper.Map<IList<EventTypeDto>>(eventTypes);
            return Ok(eventTypeDtos);
        }
    }
}

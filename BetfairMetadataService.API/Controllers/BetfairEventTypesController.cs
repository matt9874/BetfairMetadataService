using AutoMapper;
using BetfairMetadataService.API.Models.Betfair;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.WebRequests.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetfairMetadataService.API.Controllers
{
    [Route("api/betfairEventTypes")]
    [ApiController]
    public class BetfairEventTypesController: ControllerBase
    {
        private readonly IRequestInvokerAsync _requestInvoker;
        private readonly IMapper _mapper;

        public BetfairEventTypesController(IRequestInvokerAsync requestInvoker, IMapper mapper)
        {
            _requestInvoker = requestInvoker;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetEventTypes()
        { 
            IList<EventTypeResult> eventTypeResults = await _requestInvoker.Invoke<IList<EventTypeResult>>(BetfairMethod.ListEventTypes);
            IList<EventTypeDto> eventTypeDtos = _mapper.Map<IList<EventTypeDto>>(eventTypeResults);
            return Ok(eventTypeDtos);
        }
    }
}

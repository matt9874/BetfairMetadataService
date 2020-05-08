using AutoMapper;
using BetfairMetadataService.API.Models.External;
using BetfairMetadataService.API.Models.FetchRoots;
using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.Domain.FetchRoots;
using BetfairMetadataService.WebRequests.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetfairMetadataService.API.Controllers
{
    [Route("api/eventTypeMarketTypeFetchRoots")]
    [ApiController]
    public class EventTypeMarketTypeFetchRootsController : ControllerBase
    {
        private readonly IRequestInvokerAsync _requestInvoker;
        private readonly IMapper _mapper;
        private readonly IReader<EventTypeMarketType, Tuple<string, string>> _eventTypeMarketTypeReader;
        private readonly ISaver<EventTypeMarketType> _eventTypeMarketTypeSaver;

        public EventTypeMarketTypeFetchRootsController(IRequestInvokerAsync requestInvoker, IMapper mapper,
            IReader<EventTypeMarketType, Tuple<string, string>> eventTypeMarketTypeReader,
            ISaver<EventTypeMarketType> eventTypeMarketTypeSaver)
        {
            _requestInvoker = requestInvoker;
            _mapper = mapper;
            _eventTypeMarketTypeReader = eventTypeMarketTypeReader;
            _eventTypeMarketTypeSaver = eventTypeMarketTypeSaver;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEventTypeMarketTypeFetchRoot(EventTypeMarketTypeCreationDto eventTypeMarketType)
        {
            MarketFilter marketFilter = new MarketFilter()
            {
                EventTypeIds = new HashSet<string>() { eventTypeMarketType.EventTypeId }
            };

            IList<MarketTypeResult> marketTypeResults = await _requestInvoker.Invoke<IList<MarketTypeResult>>(
                BetfairMethod.ListMarketTypes,
                new Dictionary<string, object>()
                {
                    { "filter", marketFilter }
                });

            if (!marketTypeResults.Any(mtr => mtr.MarketType == eventTypeMarketType.MarketType))
                return Conflict($"There are no markets with type {eventTypeMarketType.MarketType} in the eventType with id {eventTypeMarketType.EventTypeId} in the Betfair system");

            if (_eventTypeMarketTypeReader.Read(Tuple.Create(eventTypeMarketType.EventTypeId, eventTypeMarketType.MarketType)) != null)
                return Conflict($"The fetch root object with eventTypeId {eventTypeMarketType.EventTypeId} and marketType {eventTypeMarketType.MarketType} has already been saved.");

            var fetchRoot = new EventTypeMarketType()
            {
                EventTypeId = eventTypeMarketType.EventTypeId,
                MarketType = eventTypeMarketType.MarketType
            };
            await _eventTypeMarketTypeSaver.Save(fetchRoot);

            EventTypeMarketTypeDto fetchRootDto = _mapper.Map<EventTypeMarketTypeDto>(fetchRoot);

            return CreatedAtRoute(
                "GetEventTypeMarketTypeFetchRoot",
                new
                {
                    eventTypeId = eventTypeMarketType.EventTypeId,
                    marketType = eventTypeMarketType.MarketType
                },
                fetchRootDto);
        }

        [HttpGet("eventTypes/{eventTypeId}/marketTypes/{marketType}", Name = "GetEventTypeMarketTypeFetchRoot")]
        public async Task<IActionResult> GetEventTypeMarketTypeFetchRoot(string eventTypeId, string marketType)
        {
            MarketFilter marketFilter = new MarketFilter()
            {
                EventTypeIds = new HashSet<string>() { eventTypeId }
            };

            IList<MarketTypeResult> marketTypeResults = await _requestInvoker.Invoke<IList<MarketTypeResult>>(
                BetfairMethod.ListMarketTypes,
                new Dictionary<string, object>()
                {
                    { "filter", marketFilter }
                });

            MarketTypeResult marketTypeResult = marketTypeResults.FirstOrDefault(mtr => mtr.MarketType == marketType);
            MarketTypeDto marketTypeDto = _mapper.Map<MarketTypeDto>(marketTypeResult);
            return Ok(marketTypeDto);
        }
    }
}

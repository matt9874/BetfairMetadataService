using BetfairMetadataService.API.Filters;
using BetfairMetadataService.API.Models.FetchRoots;
using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.External;
using BetfairMetadataService.Domain.FetchRoots;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetfairMetadataService.API.Controllers
{
    [Route("api/dataProviders/{dataProviderId}/eventTypeFetchRoots/")]
    [ApiController]
    public class EventTypeMarketTypeFetchRootsController : ControllerBase
    {
        private readonly Func<int, IMarketTypesService> _marketTypesServiceFactory;
        private readonly IReader<EventTypeMarketType, Tuple<int,string,string>> _eventTypeMarketTypeReader;
        private readonly IBatchReader<EventTypeMarketType> _eventTypeMarketTypeBatchReader;
        private readonly ISaver<EventTypeMarketType> _eventTypeMarketTypeSaver;
        private readonly IDeleter<EventTypeMarketType> _eventTypeMarketTypeDeleter;
        private readonly IReader<DataProvider, int> _dataProviderReader;
        private readonly IReader<EventType, string> _eventTypeReader;

        public EventTypeMarketTypeFetchRootsController(IReader<DataProvider, int> dataProviderReader, 
            IReader<EventType, string> eventTypeReader,
            IBatchReader<EventTypeMarketType> eventTypeMarketTypeBatchReader,
            Func<int, IMarketTypesService> marketTypesServiceFactory,
            IReader<EventTypeMarketType, Tuple<int, string, string>> eventTypeMarketTypeReader, 
            ISaver<EventTypeMarketType> eventTypeMarketTypeSaver,
            IDeleter<EventTypeMarketType> eventTypeMarketTypeDeleter)
        {
            _dataProviderReader = dataProviderReader;
            _eventTypeReader = eventTypeReader;
            _eventTypeMarketTypeBatchReader = eventTypeMarketTypeBatchReader;
            _marketTypesServiceFactory = marketTypesServiceFactory;
            _eventTypeMarketTypeReader = eventTypeMarketTypeReader;
            _eventTypeMarketTypeSaver = eventTypeMarketTypeSaver;
            _eventTypeMarketTypeDeleter = eventTypeMarketTypeDeleter;
        }

        [HttpPost("{eventTypeId}/marketTypes/")]
        [EventTypeMarketTypeResultFilter]
        public async Task<IActionResult> CreateEventTypeMarketTypeFetchRoot([FromRoute]int dataProviderId, [FromRoute]string eventTypeId,
            [FromBody]EventTypeMarketTypeCreationDto eventTypeMarketType)
        {
            DataProvider dataProvider = await _dataProviderReader.Read(dataProviderId);
            if (dataProvider == null)
                return NotFound($"Unable to find dataProvider with id {dataProviderId}");

            EventType eventType = await _eventTypeReader.Read(eventTypeId);
            if (eventType == null)
                return NotFound($"Unable to find eventType with id {eventTypeId}");

            IMarketTypesService marketTypesService = _marketTypesServiceFactory?.Invoke(dataProviderId);
            IEnumerable<MarketType> marketTypes = await marketTypesService.GetMarketTypesByEventTypeId(eventTypeId);

            if (!marketTypes.Any(mtr => mtr.Name == eventTypeMarketType.MarketType))
                return NotFound($"There are no markets with type {eventTypeMarketType.MarketType} in the eventType with id {eventTypeId} in the Betfair system");

            if (await _eventTypeMarketTypeReader.Read(Tuple.Create(dataProviderId, eventTypeId, eventTypeMarketType.MarketType)) != null)
                return Conflict($"The fetch root object with eventTypeId {eventTypeId} and marketType {eventTypeMarketType.MarketType} has already been saved.");

            var fetchRoot = new EventTypeMarketType()
            {
                DataProviderId = dataProviderId,
                EventTypeId = eventTypeId,
                MarketType = eventTypeMarketType.MarketType
            };
            await _eventTypeMarketTypeSaver.Save(fetchRoot);

            return CreatedAtRoute(
                "GetEventTypeMarketTypeFetchRoot",
                new
                {
                    dataProviderId = dataProviderId,
                    eventTypeId = eventTypeId,
                    marketType = eventTypeMarketType.MarketType
                },
                fetchRoot);
        }

        [HttpGet("{eventTypeId}/marketTypes/{marketType}", Name = "GetEventTypeMarketTypeFetchRoot")]
        [EventTypeMarketTypeResultFilter]
        public async Task<IActionResult> GetEventTypeMarketTypeFetchRoot(int dataProviderId, string eventTypeId, string marketType)
        {
            EventTypeMarketType eventTypeMarketType = await _eventTypeMarketTypeReader.Read(Tuple.Create(dataProviderId, eventTypeId, marketType));

            if (eventTypeMarketType == null)
                return NotFound($"Unable to find fetch root with dataProviderId of {dataProviderId}, eventTypeId of {eventTypeId} and marketType of {marketType}");

            return Ok(eventTypeMarketType);
        }

        [HttpDelete("{eventTypeId}/marketTypes/{marketType}")]
        public async Task<IActionResult> DeleteEventTypeMarketTypeFetchRoot(int dataProviderId, string eventTypeId, string marketType)
        {
            if(await _eventTypeMarketTypeReader.Read(Tuple.Create(dataProviderId, eventTypeId, marketType)) == null)
                return NotFound($"The fetch root object with dataProviderId {dataProviderId}, eventTypeId {eventTypeId} and marketType {marketType} cannot be found.");

            var eventTypeMarketType = new EventTypeMarketType()
            { 
                DataProviderId = dataProviderId,
                EventTypeId = eventTypeId,
                MarketType = marketType
            };

            await _eventTypeMarketTypeDeleter.Delete(eventTypeMarketType);

            return NoContent();
        }

        [HttpGet]
        [EventTypeMarketTypesResultFilter]
        public async Task<IActionResult> GetEventTypeMarketTypeFetchRoots(int dataProviderId)
        {
            DataProvider dataProvider = await _dataProviderReader.Read(dataProviderId);
            if (dataProvider == null)
                return NotFound($"Unable to find dataProvider with id {dataProviderId}");

            IEnumerable<EventTypeMarketType> eventTypeMarketTypes = await _eventTypeMarketTypeBatchReader.Read(etmt => etmt.DataProviderId == dataProviderId);

            if (eventTypeMarketTypes == null)
                return StatusCode(500);

            return Ok(eventTypeMarketTypes);
        }

        [HttpGet("{eventTypeId}/marketTypes")]
        [EventTypeMarketTypesResultFilter]
        public async Task<IActionResult> GetEventTypeMarketTypeFetchRootsForEventType(int dataProviderId, string eventTypeId)
        {
            DataProvider dataProvider = await _dataProviderReader.Read(dataProviderId);
            if (dataProvider == null)
                return NotFound($"Unable to find dataProvider with id {dataProviderId}");
            
            IEnumerable<EventTypeMarketType> eventTypeMarketTypes = await _eventTypeMarketTypeBatchReader.Read(etmt => 
                etmt.DataProviderId == dataProviderId && etmt.EventTypeId == eventTypeId);

            if (eventTypeMarketTypes == null)
                return StatusCode(500);

            return Ok(eventTypeMarketTypes);
        }
    }
}

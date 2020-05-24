using BetfairMetadataService.API.Filters;
using BetfairMetadataService.API.Models.FetchRoots;
using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.DataAccess.Interfaces.Repositories;
using BetfairMetadataService.Domain.External;
using BetfairMetadataService.Domain.FetchRoots;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetfairMetadataService.API.Controllers
{
    [Route("api/dataProviders/{dataProviderId}/competitionFetchRoots/")]
    [ApiController]
    public class CompetitionMarketTypeFetchRootsController : ControllerBase
    {
        private readonly Func<int, IExternalMarketTypesRepository> _marketTypesRepositoryFactory;
        private readonly IReader<CompetitionMarketType, Tuple<int, string, string>> _competitionMarketTypeReader;
        private readonly IBatchReader<CompetitionMarketType> _competitionMarketTypeBatchReader;
        private readonly ISaver<CompetitionMarketType> _competitionMarketTypeSaver;
        private readonly IDeleter<CompetitionMarketType> _competitionMarketTypeDeleter;
        private readonly IReader<DataProvider, int> _dataProviderReader;
        private readonly IReader<Competition, string> _competitionReader;

        public CompetitionMarketTypeFetchRootsController(IReader<DataProvider, int> dataProviderReader,
            IReader<Competition, string> competitionReader,
            IBatchReader<CompetitionMarketType> competitionMarketTypeBatchReader,
            Func<int, IExternalMarketTypesRepository> marketTypesRepositoryFactory,
            IReader<CompetitionMarketType, Tuple<int, string, string>> competitionMarketTypeReader,
            ISaver<CompetitionMarketType> competitionMarketTypeSaver,
            IDeleter<CompetitionMarketType> competitionMarketTypeDeleter)
        {
            _dataProviderReader = dataProviderReader;
            _competitionReader = competitionReader;
            _competitionMarketTypeBatchReader = competitionMarketTypeBatchReader;
            _marketTypesRepositoryFactory = marketTypesRepositoryFactory;
            _competitionMarketTypeReader = competitionMarketTypeReader;
            _competitionMarketTypeSaver = competitionMarketTypeSaver;
            _competitionMarketTypeDeleter = competitionMarketTypeDeleter;
        }

        [HttpPost("{competitionId}/marketTypes/")]
        [CompetitionMarketTypeResultFilter]
        public async Task<IActionResult> CreateCompetitionMarketTypeFetchRoot([FromRoute]int dataProviderId, [FromRoute]string competitionId,
            [FromBody]CompetitionMarketTypeCreationDto competitionMarketType)
        {
            DataProvider dataProvider = await _dataProviderReader.Read(dataProviderId);
            if (dataProvider == null)
                return NotFound($"Unable to find dataProvider with id {dataProviderId}");

            Competition competition = await _competitionReader.Read(competitionId);
            if (competition == null)
                return NotFound($"Unable to find competition with id {competitionId}");

            IExternalMarketTypesRepository marketTypesRepository = _marketTypesRepositoryFactory?.Invoke(dataProviderId);
            IEnumerable<MarketType> marketTypes = await marketTypesRepository.GetMarketTypesByCompetitionId(competitionId);

            if (!marketTypes.Any(mtr => mtr.Name == competitionMarketType.MarketType))
                return NotFound($"There are no markets with type {competitionMarketType.MarketType} in the competition with id {competitionId} in the Betfair system");

            if (await _competitionMarketTypeReader.Read(Tuple.Create(dataProviderId, competitionId, competitionMarketType.MarketType)) != null)
                return Conflict($"The fetch root object with competitionId {competitionId} and marketType {competitionMarketType.MarketType} has already been saved.");

            var fetchRoot = new CompetitionMarketType()
            {
                DataProviderId = dataProviderId,
                CompetitionId = competitionId,
                MarketType = competitionMarketType.MarketType
            };
            await _competitionMarketTypeSaver.Save(fetchRoot);

            return CreatedAtRoute(
                "GetCompetitionMarketTypeFetchRoot",
                new
                {
                    dataProviderId = dataProviderId,
                    competitionId = competitionId,
                    marketType = competitionMarketType.MarketType
                },
                fetchRoot);
        }

        [HttpGet("{competitionId}/marketTypes/{marketType}", Name = "GetCompetitionMarketTypeFetchRoot")]
        [CompetitionMarketTypeResultFilter]
        public async Task<IActionResult> GetCompetitionMarketTypeFetchRoot(int dataProviderId, string competitionId, string marketType)
        {
            CompetitionMarketType competitionMarketType = await _competitionMarketTypeReader.Read(Tuple.Create(dataProviderId, competitionId, marketType));

            if (competitionMarketType == null)
                return NotFound($"Unable to find fetch root with dataProviderId of {dataProviderId}, competitionId of {competitionId} and marketType of {marketType}");

            return Ok(competitionMarketType);
        }

        [HttpDelete("{competitionId}/marketTypes/{marketType}")]
        public async Task<IActionResult> DeleteCompetitionMarketTypeFetchRoot(int dataProviderId, string competitionId, string marketType)
        {
            if (await _competitionMarketTypeReader.Read(Tuple.Create(dataProviderId, competitionId, marketType)) == null)
                return NotFound($"The fetch root object with dataProviderId {dataProviderId}, competitionId {competitionId} and marketType {marketType} cannot be found.");

            var competitionMarketType = new CompetitionMarketType()
            {
                DataProviderId = dataProviderId,
                CompetitionId = competitionId,
                MarketType = marketType
            };

            await _competitionMarketTypeDeleter.Delete(competitionMarketType);

            return NoContent();
        }

        [HttpGet]
        [CompetitionMarketTypesResultFilter]
        public async Task<IActionResult> GetCompetitionMarketTypeFetchRoots(int dataProviderId)
        {
            DataProvider dataProvider = await _dataProviderReader.Read(dataProviderId);
            if (dataProvider == null)
                return NotFound($"Unable to find dataProvider with id {dataProviderId}");

            IEnumerable<CompetitionMarketType> competitionMarketTypes = await _competitionMarketTypeBatchReader.Read(etmt => etmt.DataProviderId == dataProviderId);

            if (competitionMarketTypes == null)
                return StatusCode(500);

            return Ok(competitionMarketTypes);
        }

        [HttpGet("{competitionId}/marketTypes")]
        [CompetitionMarketTypesResultFilter]
        public async Task<IActionResult> GetCompetitionMarketTypeFetchRootsForCompetition(int dataProviderId, string competitionId)
        {
            DataProvider dataProvider = await _dataProviderReader.Read(dataProviderId);
            if (dataProvider == null)
                return NotFound($"Unable to find dataProvider with id {dataProviderId}");

            IEnumerable<CompetitionMarketType> competitionMarketTypes = await _competitionMarketTypeBatchReader.Read(etmt =>
                etmt.DataProviderId == dataProviderId && etmt.CompetitionId == competitionId);

            if (competitionMarketTypes == null)
                return StatusCode(500);

            return Ok(competitionMarketTypes);
        }

        [HttpOptions]
        public Task<IActionResult> GetCompetitionMarketTypesOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,DELETE");
            IActionResult actionResult = Ok();
            return Task.FromResult(actionResult);
        }
    }
}

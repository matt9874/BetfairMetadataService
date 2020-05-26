using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.FetchRoots;
using System.Threading.Tasks;
using BetfairMetadataService.DataAccess.Interfaces.Repositories;
using System;
using BetfairMetadataService.Domain.External;
using BetfairMetadataService.API.WorkerInterfaces;
using System.Collections.Generic;

namespace BetfairMetadataService.API.Workers
{
    public class BetfairCompetitionWorker : ICompetitionMarketTypeWorker
    {
        private readonly IReader<DataProvider, int> _externalDataProviderReader;
        private readonly Func<int, IExternalCompetitionsRepository> _externalCompetitionsRepositoryFactory;
        private readonly Func<int, IExternalMarketTypesRepository> _externalMarketTypesRepositoryFactory;
        private readonly Func<int, IExternalEventTypesRepository> _externalEventTypesRepositoryFactory;
        private readonly Func<int, IExternalEventsRepository> _externalEventsRepositoryFactory;
        private readonly Func<int, IExternalMarketsRepository> _externalMarketsRepositoryFactory;

        public BetfairCompetitionWorker(IReader<DataProvider, int> dataProviderReader,
            Func<int, IExternalCompetitionsRepository> competitionsRepositoryFactory,
            Func<int, IExternalMarketTypesRepository> marketTypesRepositoryFactory,
            Func<int, IExternalEventTypesRepository> eventTypesRepositoryFactory,
            Func<int, IExternalEventsRepository> eventsRepositoryFactory,
            Func<int, IExternalMarketsRepository> externalMarketsRepositoryFactory)
        {
            _externalDataProviderReader = dataProviderReader;
            _externalCompetitionsRepositoryFactory = competitionsRepositoryFactory;
            _externalMarketTypesRepositoryFactory = marketTypesRepositoryFactory;
            _externalEventTypesRepositoryFactory = eventTypesRepositoryFactory;
            _externalEventsRepositoryFactory = eventsRepositoryFactory;
            _externalMarketsRepositoryFactory = externalMarketsRepositoryFactory;
        }

        public async Task DoWork(CompetitionMarketType competitionMarketType)
        {
            DataProvider dataProvider = await _externalDataProviderReader.Read(competitionMarketType.DataProviderId);
            if (dataProvider == null)
                throw new Exception($"No data provider found with id of {competitionMarketType.DataProviderId}");
            //TODO: check data provider is in DB and insert if it isn't

            IExternalCompetitionsRepository competitionsRepository = _externalCompetitionsRepositoryFactory?.Invoke(competitionMarketType.DataProviderId);
            Competition competition = await competitionsRepository.GetCompetition(competitionMarketType.CompetitionId);
            if(competition == null)
                throw new Exception($"No competition found with id of {competitionMarketType.CompetitionId}");
            //TODO: check competition is in DB and insert if it isn't

            IExternalMarketTypesRepository marketTypesRepository = _externalMarketTypesRepositoryFactory?.Invoke(competitionMarketType.DataProviderId);
            MarketType marketTypeForCompetition = await marketTypesRepository.GetMarketTypeForCompetition(
                competitionMarketType.CompetitionId,
                competitionMarketType.MarketType);
            if (marketTypeForCompetition == null)
                throw new Exception($"No {competitionMarketType.MarketType} market type found for competition with id {competitionMarketType.CompetitionId}");
            //TODO: check competition is in DB and insert if it isn't

            IExternalEventTypesRepository eventTypesRepository = _externalEventTypesRepositoryFactory?.Invoke(competitionMarketType.DataProviderId);
            EventType eventType = await eventTypesRepository.GetEventTypeForCompetition(competitionMarketType.CompetitionId);
            if (eventType != null)
            {
                //Upsert eventType
            }

            IExternalEventsRepository eventsRepository = _externalEventsRepositoryFactory?.Invoke(competitionMarketType.DataProviderId);
            IEnumerable<Event> events = await eventsRepository.GetEventsByCompetitionIdAndMarketType(
                competitionMarketType.CompetitionId, 
                competitionMarketType.MarketType);
            foreach (var ev in events ?? new Event[0])
            {
                //Upsert event
                IExternalMarketsRepository marketsRepository = _externalMarketsRepositoryFactory?.Invoke(competitionMarketType.DataProviderId);
                Market market = await marketsRepository.GetMarketForEventAndMarketType(
                    ev.Id,
                    competitionMarketType.MarketType);
                //if not  null
                //    Upsert market
            }
        }

    }
}

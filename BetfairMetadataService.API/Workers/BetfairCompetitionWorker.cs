using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.FetchRoots;
using System.Threading.Tasks;
using BetfairMetadataService.DataAccess.Interfaces.Repositories;
using System;
using BetfairMetadataService.Domain.External;
using BetfairMetadataService.API.WorkerInterfaces;
using System.Collections.Generic;
using AutoMapper;

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
        private readonly IMapper _mapper;
        private readonly IEntityUpserter _upserter;

        public BetfairCompetitionWorker(IReader<DataProvider, int> dataProviderReader,
            Func<int, IExternalCompetitionsRepository> competitionsRepositoryFactory,
            Func<int, IExternalMarketTypesRepository> marketTypesRepositoryFactory,
            Func<int, IExternalEventTypesRepository> eventTypesRepositoryFactory,
            Func<int, IExternalEventsRepository> eventsRepositoryFactory,
            Func<int, IExternalMarketsRepository> externalMarketsRepositoryFactory,
            IMapper mapper,
            IEntityUpserter upserter)
        {
            _externalDataProviderReader = dataProviderReader;
            _externalCompetitionsRepositoryFactory = competitionsRepositoryFactory;
            _externalMarketTypesRepositoryFactory = marketTypesRepositoryFactory;
            _externalEventTypesRepositoryFactory = eventTypesRepositoryFactory;
            _externalEventsRepositoryFactory = eventsRepositoryFactory;
            _externalMarketsRepositoryFactory = externalMarketsRepositoryFactory;
            _mapper = mapper;
            _upserter = upserter;
        }

        public async Task DoWork(CompetitionMarketType competitionMarketType)
        {
            DataProvider externalDataProvider = await _externalDataProviderReader.Read(competitionMarketType.DataProviderId);
            if (externalDataProvider == null)
                throw new Exception($"No data provider found with id of {competitionMarketType.DataProviderId}");
            Domain.Internal.DataProvider dataProvider = _mapper.Map<Domain.Internal.DataProvider>(externalDataProvider);
            await _upserter.Upsert(dataProvider);

            IExternalCompetitionsRepository competitionsRepository = _externalCompetitionsRepositoryFactory?.Invoke(competitionMarketType.DataProviderId);
            Competition externalCompetition = await competitionsRepository.GetCompetition(competitionMarketType.CompetitionId);
            if(externalCompetition == null)
                throw new Exception($"No competition found with id of {competitionMarketType.CompetitionId}");

            IExternalMarketTypesRepository marketTypesRepository = _externalMarketTypesRepositoryFactory?.Invoke(competitionMarketType.DataProviderId);
            MarketType marketTypeForCompetition = await marketTypesRepository.GetMarketTypeForCompetition(
                competitionMarketType.CompetitionId,
                competitionMarketType.MarketType);
            if (marketTypeForCompetition == null)
                throw new Exception($"No {competitionMarketType.MarketType} market type found for competition with id {competitionMarketType.CompetitionId}");
            
            IExternalEventTypesRepository eventTypesRepository = _externalEventTypesRepositoryFactory?.Invoke(competitionMarketType.DataProviderId);
            EventType externalEventType = await eventTypesRepository.GetEventTypeForCompetition(competitionMarketType.CompetitionId);
            if (externalEventType != null)
            {
                Domain.Internal.EventType eventType = _mapper.Map<Domain.Internal.EventType>(externalEventType);
                await _upserter.Upsert(eventType);
            }

            Domain.Internal.Competition competition = _mapper.Map<Domain.Internal.Competition>(externalCompetition);
            if (competition != null)
            {
                competition.EventTypeId = externalEventType?.Id;
                await _upserter.Upsert(competition);
            }
            IExternalEventsRepository eventsRepository = _externalEventsRepositoryFactory?.Invoke(competitionMarketType.DataProviderId);
            IEnumerable<Event> events = await eventsRepository.GetEventsByCompetitionIdAndMarketType(
                competitionMarketType.CompetitionId, 
                competitionMarketType.MarketType);
            foreach (var externalEvent in events ?? new Event[0])
            {
                Domain.Internal.Event internalEvent = _mapper.Map<Domain.Internal.Event>(externalEvent);
                if (internalEvent != null)
                {
                    internalEvent.CompetitionId = competition?.Id;
                    await _upserter.Upsert(internalEvent);
                }

                IExternalMarketsRepository marketsRepository = _externalMarketsRepositoryFactory?.Invoke(competitionMarketType.DataProviderId);
                Market externalMarket = await marketsRepository.GetMarketForEventAndMarketType(
                    externalEvent.Id,
                    competitionMarketType.MarketType);
                Domain.Internal.Market market = _mapper.Map<Domain.Internal.Market>(externalMarket);
                if (market != null)
                {
                    market.EventId = internalEvent?.Id;
                    await _upserter.Upsert(market);
                }
                foreach (var externalRunner in externalMarket.Runners)
                {
                    Domain.Internal.Selection selection = _mapper.Map<Domain.Internal.Selection>(externalRunner);
                    if (selection != null)
                    {
                        selection.MarketId = market?.Id;
                        await _upserter.Upsert(selection);
                    }
                }
            }
        }
    }
}

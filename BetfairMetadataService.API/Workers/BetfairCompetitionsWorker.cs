using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.FetchRoots;
using System.Collections.Generic;
using System.Threading.Tasks;
using BetfairMetadataService.API.WorkerInterfaces;

namespace BetfairMetadataService.API.Workers
{
    public class BetfairCompetitionsWorker : IWorker
    {
        private readonly IBatchReader<CompetitionMarketType> _competitionMarketTypesReader;
        private readonly ICompetitionMarketTypeWorker _competitionMarketTypeWorker;

        public BetfairCompetitionsWorker(IBatchReader<CompetitionMarketType> competitionMarketTypesReader,
            ICompetitionMarketTypeWorker competitionMarketTypeWorker)
        {
            _competitionMarketTypesReader = competitionMarketTypesReader;
            _competitionMarketTypeWorker = competitionMarketTypeWorker;
        }

        public async Task DoWork()
        {
            IEnumerable<CompetitionMarketType> fetchRoots = await _competitionMarketTypesReader.Read(cmt => true);

            foreach (var competitionMarketType in fetchRoots ?? new CompetitionMarketType[0])
            {
                await _competitionMarketTypeWorker.DoWork(competitionMarketType);
            }
        }
    }
}

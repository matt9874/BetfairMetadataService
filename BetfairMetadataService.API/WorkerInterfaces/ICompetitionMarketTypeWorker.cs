using BetfairMetadataService.Domain.FetchRoots;
using System.Threading.Tasks;

namespace BetfairMetadataService.API.WorkerInterfaces
{
    public interface ICompetitionMarketTypeWorker
    {
        Task DoWork(CompetitionMarketType competitionMarketType);
    }
}

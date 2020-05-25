using System.Threading.Tasks;

namespace BetfairMetadataService.API.WorkerInterfaces
{
    public interface IWorker
    {
        Task DoWork();
    }
}

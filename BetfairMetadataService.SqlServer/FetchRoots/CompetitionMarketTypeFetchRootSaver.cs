using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.FetchRoots;
using System.Threading.Tasks;

namespace BetfairMetadataService.SqlServer.FetchRoots
{
    public class CompetitionMarketTypeFetchRootSaver : ISaver<CompetitionMarketType>
    {
        private readonly BetfairMetadataServiceContext _context;

        public CompetitionMarketTypeFetchRootSaver(BetfairMetadataServiceContext context)
        {
            _context = context;
        }

        public async Task Save(CompetitionMarketType entity)
        {
            await _context.CompetitionMarketTypeFetchRoots.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}

using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.FetchRoots;
using System;
using System.Threading.Tasks;

namespace BetfairMetadataService.SqlServer.FetchRoots
{
    public class CompetitionMarketTypeFetchRootReader : IReader<CompetitionMarketType, Tuple<int, string, string>>
    {
        private readonly BetfairMetadataServiceContext _context;

        public CompetitionMarketTypeFetchRootReader(BetfairMetadataServiceContext context)
        {
            _context = context;
        }

        public async Task<CompetitionMarketType> Read(Tuple<int, string, string> id)
        {
            return await _context.CompetitionMarketTypeFetchRoots.FindAsync(
                    id.Item1, id.Item2, id.Item3);
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace BetfairMetadataService.SqlServer
{
    public class BetfairMetadataServiceContext: DbContext
    {
        public BetfairMetadataServiceContext(DbContextOptions<BetfairMetadataServiceContext> options)
            : base(options)
        {

        }
    }
}

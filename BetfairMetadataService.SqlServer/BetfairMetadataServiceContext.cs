using BetfairMetadataService.Domain.FetchRoots;
using Microsoft.EntityFrameworkCore;

namespace BetfairMetadataService.SqlServer
{
    public class BetfairMetadataServiceContext: DbContext
    {
        public BetfairMetadataServiceContext(DbContextOptions<BetfairMetadataServiceContext> options)
            : base(options)
        {

        }

        public DbSet<EventTypeMarketType> EventTypeMarketTypeFetchRoots { get; set; }
        public DbSet<CompetitionMarketType> CompetitionMarketTypeFetchRoots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventTypeMarketType>()
                .HasKey(etmt => new { etmt.DataProviderId, etmt.EventTypeId, etmt.MarketType });

            modelBuilder.Entity<CompetitionMarketType>()
                .HasKey(cmt => new { cmt.DataProviderId, cmt.CompetitionId, cmt.MarketType });
        }
    }
}

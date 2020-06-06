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

        public DbSet<Domain.Internal.DataProvider> DataProviders { get; set; }
        public DbSet<Domain.Internal.EventType> EventTypes { get; set; }
        public DbSet<Domain.Internal.Competition> Competitions { get; set; }
        public DbSet<Domain.Internal.Event> Events { get; set; }
        public DbSet<Domain.Internal.Market> Markets { get; set; }
        public DbSet<Domain.Internal.Selection> Selections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventTypeMarketType>()
                .HasKey(etmt => new { etmt.DataProviderId, etmt.EventTypeId, etmt.MarketType });

            modelBuilder.Entity<CompetitionMarketType>()
                .HasKey(cmt => new { cmt.DataProviderId, cmt.CompetitionId, cmt.MarketType });
        
            modelBuilder.Entity<Domain.Internal.Competition>()
                .HasIndex(b => b.EventTypeId);
            modelBuilder.Entity<Domain.Internal.Event>()
                .HasIndex(b => b.CompetitionId);
        }
    }
}

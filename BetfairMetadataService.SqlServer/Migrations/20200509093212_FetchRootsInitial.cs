using Microsoft.EntityFrameworkCore.Migrations;

namespace BetfairMetadataService.SqlServer.Migrations
{
    public partial class FetchRootsInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompetitionMarketTypeFetchRoots",
                columns: table => new
                {
                    DataProviderId = table.Column<int>(nullable: false),
                    CompetitionId = table.Column<string>(nullable: false),
                    MarketType = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionMarketTypeFetchRoots", x => new { x.DataProviderId, x.CompetitionId, x.MarketType });
                });

            migrationBuilder.CreateTable(
                name: "EventTypeMarketTypeFetchRoots",
                columns: table => new
                {
                    DataProviderId = table.Column<int>(nullable: false),
                    EventTypeId = table.Column<string>(nullable: false),
                    MarketType = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypeMarketTypeFetchRoots", x => new { x.DataProviderId, x.EventTypeId, x.MarketType });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitionMarketTypeFetchRoots");

            migrationBuilder.DropTable(
                name: "EventTypeMarketTypeFetchRoots");
        }
    }
}

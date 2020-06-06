using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BetfairMetadataService.SqlServer.Migrations
{
    public partial class internalEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    EventTypeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataProviders",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true),
                    Timezone = table.Column<string>(nullable: true),
                    Venue = table.Column<string>(nullable: true),
                    OpenDate = table.Column<DateTime>(nullable: true),
                    CompetitionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Markets",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MarketName = table.Column<string>(nullable: true),
                    MarketDataDelayed = table.Column<bool>(nullable: true),
                    EventId = table.Column<string>(nullable: true),
                    PersistenceEnabled = table.Column<bool>(nullable: true),
                    BspMarket = table.Column<bool>(nullable: true),
                    MarketTime = table.Column<DateTime>(nullable: false),
                    SuspendTime = table.Column<DateTime>(nullable: true),
                    SettleTime = table.Column<DateTime>(nullable: true),
                    BettingType = table.Column<int>(nullable: false),
                    TurnInPlayEnabled = table.Column<bool>(nullable: true),
                    MarketType = table.Column<string>(nullable: true),
                    Regulator = table.Column<string>(nullable: true),
                    MarketBaseRate = table.Column<double>(nullable: true),
                    DiscountAllowed = table.Column<bool>(nullable: true),
                    Wallet = table.Column<string>(nullable: true),
                    Rules = table.Column<string>(nullable: true),
                    RulesHasDate = table.Column<bool>(nullable: true),
                    Clarifications = table.Column<string>(nullable: true),
                    TotalMatched = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Markets_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Selections",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MarketId = table.Column<string>(nullable: true),
                    RunnerName = table.Column<string>(nullable: true),
                    Handicap = table.Column<double>(nullable: false),
                    SortPriority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Selections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Selections_Markets_MarketId",
                        column: x => x.MarketId,
                        principalTable: "Markets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_EventTypeId",
                table: "Competitions",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CompetitionId",
                table: "Events",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Markets_EventId",
                table: "Markets",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Selections_MarketId",
                table: "Selections",
                column: "MarketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.DropTable(
                name: "DataProviders");

            migrationBuilder.DropTable(
                name: "EventTypes");

            migrationBuilder.DropTable(
                name: "Selections");

            migrationBuilder.DropTable(
                name: "Markets");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}

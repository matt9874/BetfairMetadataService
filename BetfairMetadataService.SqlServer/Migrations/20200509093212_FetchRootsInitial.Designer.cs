﻿// <auto-generated />
using BetfairMetadataService.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BetfairMetadataService.SqlServer.Migrations
{
    [DbContext(typeof(BetfairMetadataServiceContext))]
    [Migration("20200509093212_FetchRootsInitial")]
    partial class FetchRootsInitial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BetfairMetadataService.Domain.FetchRoots.CompetitionMarketType", b =>
                {
                    b.Property<int>("DataProviderId")
                        .HasColumnType("int");

                    b.Property<string>("CompetitionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MarketType")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DataProviderId", "CompetitionId", "MarketType");

                    b.ToTable("CompetitionMarketTypeFetchRoots");
                });

            modelBuilder.Entity("BetfairMetadataService.Domain.FetchRoots.EventTypeMarketType", b =>
                {
                    b.Property<int>("DataProviderId")
                        .HasColumnType("int");

                    b.Property<string>("EventTypeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MarketType")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DataProviderId", "EventTypeId", "MarketType");

                    b.ToTable("EventTypeMarketTypeFetchRoots");
                });
#pragma warning restore 612, 618
        }
    }
}

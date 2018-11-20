﻿// <auto-generated />
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Uchu.Core;

namespace Uchu.Core.Migrations
{
    [DbContext(typeof(UchuContext))]
    partial class UchuContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Uchu.Core.Character", b =>
                {
                    b.Property<long>("CharacterId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Currency");

                    b.Property<int>("CurrentArmor");

                    b.Property<int>("CurrentHealth");

                    b.Property<int>("CurrentImagination");

                    b.Property<string>("CustomName")
                        .IsRequired()
                        .HasMaxLength(33);

                    b.Property<long>("EyeStyle");

                    b.Property<long>("EyebrowStyle");

                    b.Property<bool>("FreeToPlay");

                    b.Property<long>("HairColor");

                    b.Property<long>("HairStyle");

                    b.Property<bool>("LandingByRocket");

                    b.Property<long>("LastActivity");

                    b.Property<long>("LastClone");

                    b.Property<int>("LastInstance");

                    b.Property<int>("LastZone");

                    b.Property<long>("Level");

                    b.Property<long>("Lh");

                    b.Property<int>("MaximumArmor");

                    b.Property<int>("MaximumHealth");

                    b.Property<int>("MaximumImagination");

                    b.Property<long>("MouthStyle");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(33);

                    b.Property<bool>("NameRejected");

                    b.Property<long>("PantsColor");

                    b.Property<long>("Rh");

                    b.Property<string>("Rocket")
                        .HasMaxLength(30);

                    b.Property<long>("ShirtColor");

                    b.Property<long>("ShirtStyle");

                    b.Property<long>("TotalArmorPowerUpsCollected");

                    b.Property<long>("TotalArmorRepaired");

                    b.Property<long>("TotalBricksCollected");

                    b.Property<long>("TotalCurrencyCollected");

                    b.Property<long>("TotalDamageHealed");

                    b.Property<long>("TotalDamageTaken");

                    b.Property<long>("TotalDistanceDriven");

                    b.Property<long>("TotalDistanceTraveled");

                    b.Property<long>("TotalEnemiesSmashed");

                    b.Property<long>("TotalFirstPlaceFinishes");

                    b.Property<long>("TotalImaginationPowerUpsCollected");

                    b.Property<long>("TotalImaginationRestored");

                    b.Property<long>("TotalImaginationUsed");

                    b.Property<long>("TotalLifePowerUpsCollected");

                    b.Property<long>("TotalMissionsCompleted");

                    b.Property<long>("TotalPetsTamed");

                    b.Property<long>("TotalQuickBuildsCompleted");

                    b.Property<long>("TotalRacecarBoostsActivated");

                    b.Property<long>("TotalRacecarWrecks");

                    b.Property<long>("TotalRacesFinished");

                    b.Property<long>("TotalRacingImaginationCratesSmashed");

                    b.Property<long>("TotalRacingImaginationPowerUpsCollected");

                    b.Property<long>("TotalRacingSmashablesSmashed");

                    b.Property<long>("TotalRocketsUsed");

                    b.Property<long>("TotalSmashablesSmashed");

                    b.Property<long>("TotalSuicides");

                    b.Property<long>("TotalTimeAirborne");

                    b.Property<long>("UniverseScore");

                    b.Property<long>("UserId");

                    b.HasKey("CharacterId");

                    b.HasIndex("UserId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("Uchu.Core.InventoryItem", b =>
                {
                    b.Property<long>("InventoryItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CharacterId");

                    b.Property<long>("Count");

                    b.Property<string>("ExtraInfo");

                    b.Property<int>("InventoryType");

                    b.Property<bool>("IsBound");

                    b.Property<bool>("IsEquipped");

                    b.Property<int>("LOT");

                    b.Property<int>("Slot");

                    b.HasKey("InventoryItemId");

                    b.HasIndex("CharacterId");

                    b.ToTable("InventoryItems");
                });

            modelBuilder.Entity("Uchu.Core.Mission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CharacterId");

                    b.Property<int>("CompletionCount");

                    b.Property<long>("LastCompletion");

                    b.Property<int>("MissionId");

                    b.Property<int>("State");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.ToTable("Missions");
                });

            modelBuilder.Entity("Uchu.Core.MissionTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MissionId");

                    b.Property<int>("TaskId");

                    b.Property<List<float>>("Values")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("MissionId");

                    b.ToTable("MissionTasks");
                });

            modelBuilder.Entity("Uchu.Core.User", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CharacterIndex");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(33);

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Uchu.Core.Character", b =>
                {
                    b.HasOne("Uchu.Core.User", "User")
                        .WithMany("Characters")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Uchu.Core.InventoryItem", b =>
                {
                    b.HasOne("Uchu.Core.Character", "Character")
                        .WithMany("Items")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Uchu.Core.Mission", b =>
                {
                    b.HasOne("Uchu.Core.Character", "Character")
                        .WithMany("Missions")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Uchu.Core.MissionTask", b =>
                {
                    b.HasOne("Uchu.Core.Mission", "Mission")
                        .WithMany("Tasks")
                        .HasForeignKey("MissionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

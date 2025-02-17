using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedChunksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HolidayEffectivePeriods");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "PatternType",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "WeekOfMonth",
                table: "Holidays");

            migrationBuilder.CreateTable(
                name: "ProcessedHolidaysChunks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChunkStartYear = table.Column<int>(type: "int", nullable: false),
                    ChunkEndYear = table.Column<int>(type: "int", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedHolidaysChunks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessedHolidaysChunks_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedHolidaysChunks_CountryId",
                table: "ProcessedHolidaysChunks",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessedHolidaysChunks");

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "Holidays",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "Holidays",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Holidays",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatternType",
                table: "Holidays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeekOfMonth",
                table: "Holidays",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HolidayEffectivePeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HolidayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EndYear = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartYear = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayEffectivePeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HolidayEffectivePeriods_Holidays_HolidayId",
                        column: x => x.HolidayId,
                        principalTable: "Holidays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HolidayEffectivePeriods_HolidayId",
                table: "HolidayEffectivePeriods",
                column: "HolidayId");
        }
    }
}

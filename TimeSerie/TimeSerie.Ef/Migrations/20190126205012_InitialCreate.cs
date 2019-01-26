using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeSerie.Ef.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeSerieHeader",
                columns: table => new
                {
                    TimeSerieHeaderId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TimeSerieType = table.Column<int>(nullable: false),
                    ValuesFrom = table.Column<DateTimeOffset>(nullable: true),
                    ValuesTo = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSerieHeader", x => x.TimeSerieHeaderId);
                });

            migrationBuilder.CreateTable(
                name: "TimeSerieHeaderProperty",
                columns: table => new
                {
                    TimeSerieHeaderPropertyId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TimeSerieHeaderId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSerieHeaderProperty", x => x.TimeSerieHeaderPropertyId);
                    table.ForeignKey(
                        name: "FK_TimeSerieHeaderProperty_TimeSerieHeader_TimeSerieHeaderId",
                        column: x => x.TimeSerieHeaderId,
                        principalTable: "TimeSerieHeader",
                        principalColumn: "TimeSerieHeaderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeSerieValueDecimal",
                columns: table => new
                {
                    TimeSerieValueDecimalId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TimeSerieHeaderId = table.Column<int>(nullable: false),
                    DateTimeOffset = table.Column<DateTimeOffset>(nullable: false),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSerieValueDecimal", x => x.TimeSerieValueDecimalId);
                    table.ForeignKey(
                        name: "FK_TimeSerieValueDecimal_TimeSerieHeader_TimeSerieHeaderId",
                        column: x => x.TimeSerieHeaderId,
                        principalTable: "TimeSerieHeader",
                        principalColumn: "TimeSerieHeaderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeSerieValueString",
                columns: table => new
                {
                    TimeSerieValueStringId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TimeSerieHeaderId = table.Column<int>(nullable: false),
                    DateTimeOffset = table.Column<DateTimeOffset>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSerieValueString", x => x.TimeSerieValueStringId);
                    table.ForeignKey(
                        name: "FK_TimeSerieValueString_TimeSerieHeader_TimeSerieHeaderId",
                        column: x => x.TimeSerieHeaderId,
                        principalTable: "TimeSerieHeader",
                        principalColumn: "TimeSerieHeaderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeSerieHeaderProperty_TimeSerieHeaderId",
                table: "TimeSerieHeaderProperty",
                column: "TimeSerieHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSerieValueDecimal_TimeSerieHeaderId",
                table: "TimeSerieValueDecimal",
                column: "TimeSerieHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSerieValueString_TimeSerieHeaderId",
                table: "TimeSerieValueString",
                column: "TimeSerieHeaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeSerieHeaderProperty");

            migrationBuilder.DropTable(
                name: "TimeSerieValueDecimal");

            migrationBuilder.DropTable(
                name: "TimeSerieValueString");

            migrationBuilder.DropTable(
                name: "TimeSerieHeader");
        }
    }
}

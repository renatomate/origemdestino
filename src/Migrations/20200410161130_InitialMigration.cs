using Microsoft.EntityFrameworkCore.Migrations;

namespace OrigemDestino.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Frequenter",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frequenter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    X = table.Column<int>(nullable: false),
                    Y = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocationFrequenter",
                columns: table => new
                {
                    LocationId = table.Column<int>(nullable: false),
                    FrequenterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationFrequenter", x => new { x.LocationId, x.FrequenterId });
                    table.ForeignKey(
                        name: "FK_LocationFrequenter_Frequenter_FrequenterId",
                        column: x => x.FrequenterId,
                        principalTable: "Frequenter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationFrequenter_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationFrequenter_FrequenterId",
                table: "LocationFrequenter",
                column: "FrequenterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationFrequenter");

            migrationBuilder.DropTable(
                name: "Frequenter");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}

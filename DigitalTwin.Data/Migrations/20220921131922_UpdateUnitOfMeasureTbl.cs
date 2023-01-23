using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalTwin.Data.Migrations
{
    public partial class UpdateUnitOfMeasureTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UomId",
                schema: "pivot_da_middleware_digital",
                table: "UnitOfMeasures",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UomId",
                schema: "pivot_da_middleware_digital",
                table: "UnitOfMeasures");
        }
    }
}

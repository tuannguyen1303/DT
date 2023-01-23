using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalTwin.Data.Migrations
{
    public partial class RemoveUnusedColInProductLinkDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}

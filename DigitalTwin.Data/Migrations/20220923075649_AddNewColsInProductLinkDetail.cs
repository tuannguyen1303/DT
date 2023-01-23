using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalTwin.Data.Migrations
{
    public partial class AddNewColsInProductLinkDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NorCode",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkDetails",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkDetails");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkDetails");

            migrationBuilder.DropColumn(
                name: "NorCode",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkDetails");
        }
    }
}

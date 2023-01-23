using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalTwin.Data.Migrations
{
    public partial class UpdateTblProductLinkDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkDetails",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkDetails",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Variance",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkDetails",
                type: "numeric",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Percentage",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkDetails");

            migrationBuilder.DropColumn(
                name: "Value",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkDetails");

            migrationBuilder.DropColumn(
                name: "Variance",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkDetails");
        }
    }
}

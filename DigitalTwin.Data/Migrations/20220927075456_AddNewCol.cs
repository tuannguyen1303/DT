using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalTwin.Data.Migrations
{
    public partial class AddNewCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumValues",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkDetails",
                type: "jsonb",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumValues",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkDetails");
        }
    }
}

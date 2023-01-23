using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalTwin.Data.Migrations
{
    public partial class UpdateEntityTypeTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EntityGroupName",
                schema: "pivot_da_middleware_digital",
                table: "EntityTypes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EntityId",
                schema: "pivot_da_middleware_digital",
                table: "EntityTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "pivot_da_middleware_digital",
                table: "EntityTypes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityGroupName",
                schema: "pivot_da_middleware_digital",
                table: "EntityTypes");

            migrationBuilder.DropColumn(
                name: "EntityId",
                schema: "pivot_da_middleware_digital",
                table: "EntityTypes");

            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "pivot_da_middleware_digital",
                table: "EntityTypes");
        }
    }
}

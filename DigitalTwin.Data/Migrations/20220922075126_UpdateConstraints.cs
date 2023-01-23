using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalTwin.Data.Migrations
{
    public partial class UpdateConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductLinks_ProductLinkStatuses_ProductLinkStatusId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.DropIndex(
                name: "IX_ProductLinks_ProductLinkStatusId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.DropColumn(
                name: "ProductLinkStatusId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.AlterColumn<int>(
                name: "UomId",
                schema: "pivot_da_middleware_digital",
                table: "UnitOfMeasures",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NorCode",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityTypeMasterId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NorCode",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.DropColumn(
                name: "EntityTypeMasterId",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.AlterColumn<int>(
                name: "UomId",
                schema: "pivot_da_middleware_digital",
                table: "UnitOfMeasures",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductLinkStatusId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductLinks_ProductLinkStatusId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                column: "ProductLinkStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductLinks_ProductLinkStatuses_ProductLinkStatusId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                column: "ProductLinkStatusId",
                principalSchema: "pivot_da_middleware_digital",
                principalTable: "ProductLinkStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

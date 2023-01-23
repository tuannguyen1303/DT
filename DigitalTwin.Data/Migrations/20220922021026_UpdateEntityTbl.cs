using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalTwin.Data.Migrations
{
    public partial class UpdateEntityTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CreatedBy",
                schema: "pivot_da_middleware_digital",
                table: "ValueChainTypes",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UpdatedBy",
                schema: "pivot_da_middleware_digital",
                table: "ValueChainTypes",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CreatedBy",
                schema: "pivot_da_middleware_digital",
                table: "UnitOfMeasures",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UpdatedBy",
                schema: "pivot_da_middleware_digital",
                table: "UnitOfMeasures",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CreatedBy",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkStatuses",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UpdatedBy",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkStatuses",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CreatedBy",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UpdatedBy",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CreatedBy",
                schema: "pivot_da_middleware_digital",
                table: "EntityTypes",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UpdatedBy",
                schema: "pivot_da_middleware_digital",
                table: "EntityTypes",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CreatedBy",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Depth",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "EntityParentId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UpdatedBy",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Entities_EntityParentId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                column: "EntityParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_Entities_EntityParentId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                column: "EntityParentId",
                principalSchema: "pivot_da_middleware_digital",
                principalTable: "Entities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entities_Entities_EntityParentId",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropIndex(
                name: "IX_Entities_EntityParentId",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "pivot_da_middleware_digital",
                table: "ValueChainTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "pivot_da_middleware_digital",
                table: "ValueChainTypes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "pivot_da_middleware_digital",
                table: "UnitOfMeasures");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "pivot_da_middleware_digital",
                table: "UnitOfMeasures");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkStatuses");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkStatuses");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "pivot_da_middleware_digital",
                table: "EntityTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "pivot_da_middleware_digital",
                table: "EntityTypes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "Depth",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "EntityParentId",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "pivot_da_middleware_digital",
                table: "Entities");
        }
    }
}

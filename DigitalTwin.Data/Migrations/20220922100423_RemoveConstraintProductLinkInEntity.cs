using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalTwin.Data.Migrations
{
    public partial class RemoveConstraintProductLinkInEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entities_EntityTypes_EntityTypeId",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropForeignKey(
                name: "FK_Entities_ProductLinks_ProductLinkId",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropForeignKey(
                name: "FK_Entities_ValueChainTypes_ValueChainTypeId",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropIndex(
                name: "IX_Entities_ProductLinkId",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "ProductLinkId",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.AlterColumn<Guid>(
                name: "EntityMapId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ValueChainTypeId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "EntityTypeId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLinks_EntityId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                column: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_EntityTypes_EntityTypeId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                column: "EntityTypeId",
                principalSchema: "pivot_da_middleware_digital",
                principalTable: "EntityTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_ValueChainTypes_ValueChainTypeId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                column: "ValueChainTypeId",
                principalSchema: "pivot_da_middleware_digital",
                principalTable: "ValueChainTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductLinks_Entities_EntityMapId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                column: "EntityMapId",
                principalSchema: "pivot_da_middleware_digital",
                principalTable: "Entities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entities_EntityTypes_EntityTypeId",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropForeignKey(
                name: "FK_Entities_ValueChainTypes_ValueChainTypeId",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductLinks_Entities_EntityMapId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.DropIndex(
                name: "IX_ProductLinks_EntityId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.DropColumn(
                name: "EntityId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.AlterColumn<Guid>(
                name: "EntityMapId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ValueChainTypeId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EntityTypeId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductLinkId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entities_ProductLinkId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                column: "ProductLinkId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_EntityTypes_EntityTypeId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                column: "EntityTypeId",
                principalSchema: "pivot_da_middleware_digital",
                principalTable: "EntityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_ProductLinks_ProductLinkId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                column: "ProductLinkId",
                principalSchema: "pivot_da_middleware_digital",
                principalTable: "ProductLinks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_ValueChainTypes_ValueChainTypeId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                column: "ValueChainTypeId",
                principalSchema: "pivot_da_middleware_digital",
                principalTable: "ValueChainTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

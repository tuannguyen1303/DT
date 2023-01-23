using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalTwin.Data.Migrations
{
    public partial class UpdateProductLinkTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductLinks_Entities_ChildId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductLinks_Entities_ParentId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.DropIndex(
                name: "IX_ProductLinks_ChildId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.DropIndex(
                name: "IX_ProductLinks_ParentId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.DropColumn(
                name: "ChildId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                newName: "EntityMapId");

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
                name: "FK_Entities_ProductLinks_ProductLinkId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                column: "ProductLinkId",
                principalSchema: "pivot_da_middleware_digital",
                principalTable: "ProductLinks",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entities_ProductLinks_ProductLinkId",
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

            migrationBuilder.RenameColumn(
                name: "EntityMapId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                newName: "ParentId");

            migrationBuilder.AddColumn<Guid>(
                name: "ChildId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductLinks_ChildId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLinks_ParentId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductLinks_Entities_ChildId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                column: "ChildId",
                principalSchema: "pivot_da_middleware_digital",
                principalTable: "Entities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductLinks_Entities_ParentId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                column: "ParentId",
                principalSchema: "pivot_da_middleware_digital",
                principalTable: "Entities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalTwin.Data.Migrations
{
    public partial class AddNewEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductLinks_UnitOfMeasures_UnitOfMeasureId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.DropColumn(
                name: "NorCode",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.DropColumn(
                name: "DataDate",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "IsDaily",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "IsMonthToDate",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "IsMonthly",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "IsQuarterToDate",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "IsQuarterly",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "IsRealTime",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "IsWeekly",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "IsYearEndProjection",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "IsYearToDaily",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "IsYearToMonthly",
                schema: "pivot_da_middleware_digital",
                table: "Entities");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "pivot_da_middleware_digital",
                table: "ValueChainTypes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "pivot_da_middleware_digital",
                table: "UnitOfMeasures",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "NorCode",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkStatuses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkStatuses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkStatuses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitOfMeasureId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "pivot_da_middleware_digital",
                table: "EntityTypes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                schema: "pivot_da_middleware_digital",
                table: "EntityTypes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EntityGroupName",
                schema: "pivot_da_middleware_digital",
                table: "EntityTypes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "KpiPath",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "ProductLinkDetails",
                schema: "pivot_da_middleware_digital",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductLinkId = table.Column<Guid>(type: "uuid", nullable: true),
                    Frequency = table.Column<string>(type: "text", nullable: true),
                    DataDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsRealTime = table.Column<bool>(type: "boolean", nullable: false),
                    IsDaily = table.Column<bool>(type: "boolean", nullable: false),
                    IsWeekly = table.Column<bool>(type: "boolean", nullable: false),
                    IsMonthly = table.Column<bool>(type: "boolean", nullable: false),
                    IsMonthToDate = table.Column<bool>(type: "boolean", nullable: false),
                    IsQuarterly = table.Column<bool>(type: "boolean", nullable: false),
                    IsQuarterToDate = table.Column<bool>(type: "boolean", nullable: false),
                    IsYearToDaily = table.Column<bool>(type: "boolean", nullable: false),
                    IsYearToMonthly = table.Column<bool>(type: "boolean", nullable: false),
                    IsYearEndProjection = table.Column<bool>(type: "boolean", nullable: false),
                    UomName = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UpdatedBy = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductLinkDetails", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductLinks_UnitOfMeasures_UnitOfMeasureId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                column: "UnitOfMeasureId",
                principalSchema: "pivot_da_middleware_digital",
                principalTable: "UnitOfMeasures",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductLinks_UnitOfMeasures_UnitOfMeasureId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.DropTable(
                name: "ProductLinkDetails",
                schema: "pivot_da_middleware_digital");

            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "pivot_da_middleware_digital",
                table: "ValueChainTypes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "pivot_da_middleware_digital",
                table: "UnitOfMeasures",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NorCode",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkStatuses",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkStatuses",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinkStatuses",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitOfMeasureId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NorCode",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "pivot_da_middleware_digital",
                table: "EntityTypes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                schema: "pivot_da_middleware_digital",
                table: "EntityTypes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EntityGroupName",
                schema: "pivot_da_middleware_digital",
                table: "EntityTypes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KpiPath",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataDate",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDaily",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMonthToDate",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMonthly",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsQuarterToDate",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsQuarterly",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRealTime",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWeekly",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsYearEndProjection",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsYearToDaily",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsYearToMonthly",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductLinks_UnitOfMeasures_UnitOfMeasureId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                column: "UnitOfMeasureId",
                principalSchema: "pivot_da_middleware_digital",
                principalTable: "UnitOfMeasures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

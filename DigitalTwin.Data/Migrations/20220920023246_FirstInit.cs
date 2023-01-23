using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalTwin.Data.Migrations
{
    public partial class FirstInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "pivot_da_middleware_digital");

            migrationBuilder.CreateTable(
                name: "EntityTypes",
                schema: "pivot_da_middleware_digital",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductLinkStatuses",
                schema: "pivot_da_middleware_digital",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NorCode = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductLinkStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitOfMeasures",
                schema: "pivot_da_middleware_digital",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitOfMeasures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ValueChainTypes",
                schema: "pivot_da_middleware_digital",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueChainTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entities",
                schema: "pivot_da_middleware_digital",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ValueChainTypeId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    KpiPath = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entities_EntityTypes_EntityTypeId",
                        column: x => x.EntityTypeId,
                        principalSchema: "pivot_da_middleware_digital",
                        principalTable: "EntityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entities_ValueChainTypes_ValueChainTypeId",
                        column: x => x.ValueChainTypeId,
                        principalSchema: "pivot_da_middleware_digital",
                        principalTable: "ValueChainTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductLinks",
                schema: "pivot_da_middleware_digital",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UnitOfMeasureId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductLinkStatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChildId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: true),
                    Percentage = table.Column<decimal>(type: "numeric", nullable: true),
                    Variance = table.Column<decimal>(type: "numeric", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductLinks_Entities_ChildId",
                        column: x => x.ChildId,
                        principalSchema: "pivot_da_middleware_digital",
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductLinks_Entities_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "pivot_da_middleware_digital",
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductLinks_ProductLinkStatuses_ProductLinkStatusId",
                        column: x => x.ProductLinkStatusId,
                        principalSchema: "pivot_da_middleware_digital",
                        principalTable: "ProductLinkStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductLinks_UnitOfMeasures_UnitOfMeasureId",
                        column: x => x.UnitOfMeasureId,
                        principalSchema: "pivot_da_middleware_digital",
                        principalTable: "UnitOfMeasures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entities_EntityTypeId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                column: "EntityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_ValueChainTypeId",
                schema: "pivot_da_middleware_digital",
                table: "Entities",
                column: "ValueChainTypeId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ProductLinks_ProductLinkStatusId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                column: "ProductLinkStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLinks_UnitOfMeasureId",
                schema: "pivot_da_middleware_digital",
                table: "ProductLinks",
                column: "UnitOfMeasureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductLinks",
                schema: "pivot_da_middleware_digital");

            migrationBuilder.DropTable(
                name: "Entities",
                schema: "pivot_da_middleware_digital");

            migrationBuilder.DropTable(
                name: "ProductLinkStatuses",
                schema: "pivot_da_middleware_digital");

            migrationBuilder.DropTable(
                name: "UnitOfMeasures",
                schema: "pivot_da_middleware_digital");

            migrationBuilder.DropTable(
                name: "EntityTypes",
                schema: "pivot_da_middleware_digital");

            migrationBuilder.DropTable(
                name: "ValueChainTypes",
                schema: "pivot_da_middleware_digital");
        }
    }
}

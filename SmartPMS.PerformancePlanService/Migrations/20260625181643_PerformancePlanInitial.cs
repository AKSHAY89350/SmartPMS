using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPMS.PerformancePlanService.Migrations
{
    /// <inheritdoc />
    public partial class PerformancePlanInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PerformancePlans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FinancialYear = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PlanPeriodFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlanPeriodTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformancePlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerformancePlanItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerformancePlanId = table.Column<long>(type: "bigint", nullable: false),
                    KraTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    KpiDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Weightage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MeasurementCriteria = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformancePlanItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerformancePlanItems_PerformancePlans_PerformancePlanId",
                        column: x => x.PerformancePlanId,
                        principalTable: "PerformancePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PerformancePlanItems_PerformancePlanId",
                table: "PerformancePlanItems",
                column: "PerformancePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformancePlans_EmployeeId_FinancialYear",
                table: "PerformancePlans",
                columns: new[] { "EmployeeId", "FinancialYear" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PerformancePlanItems");

            migrationBuilder.DropTable(
                name: "PerformancePlans");
        }
    }
}

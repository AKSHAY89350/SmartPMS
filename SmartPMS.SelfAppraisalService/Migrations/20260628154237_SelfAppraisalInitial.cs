using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPMS.SelfAppraisalService.Migrations
{
    /// <inheritdoc />
    public partial class SelfAppraisalInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SelfAppraisals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerformancePlanId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FinancialYear = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelfAppraisals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SelfAppraisalItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SelfAppraisalId = table.Column<long>(type: "bigint", nullable: false),
                    PerformancePlanItemId = table.Column<long>(type: "bigint", nullable: false),
                    KraTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    KpiDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Weightage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Achievement = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    SelfRating = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    EmployeeComments = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelfAppraisalItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelfAppraisalItems_SelfAppraisals_SelfAppraisalId",
                        column: x => x.SelfAppraisalId,
                        principalTable: "SelfAppraisals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SelfAppraisalItems_SelfAppraisalId_PerformancePlanItemId",
                table: "SelfAppraisalItems",
                columns: new[] { "SelfAppraisalId", "PerformancePlanItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SelfAppraisals_EmployeeId_FinancialYear",
                table: "SelfAppraisals",
                columns: new[] { "EmployeeId", "FinancialYear" });

            migrationBuilder.CreateIndex(
                name: "IX_SelfAppraisals_PerformancePlanId",
                table: "SelfAppraisals",
                column: "PerformancePlanId",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SelfAppraisalItems");

            migrationBuilder.DropTable(
                name: "SelfAppraisals");
        }
    }
}

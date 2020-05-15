using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Server.Data.Migrations
{
    public partial class PutSalaryBack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AnnualSalary",
                table: "Employees",
                nullable: false,
                defaultValue: 52_000m,
                type: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualSalary",
                table: "Employees");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Server.Data.Migrations
{
    public partial class AddPayPeriods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PayPeriodsPerYear",
                table: "Employers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayPeriodsPerYear",
                table: "Employers");
        }
    }
}

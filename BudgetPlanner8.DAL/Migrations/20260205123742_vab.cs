using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPlanner8.DAL.Migrations
{
    /// <inheritdoc />
    public partial class vab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DaysCount",
                table: "Transactions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysCount",
                table: "Transactions");
        }
    }
}

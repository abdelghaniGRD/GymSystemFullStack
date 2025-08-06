using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymSystem.Migrations
{
    /// <inheritdoc />
    public partial class addtwocol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlanName",
                table: "subscriptions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PlanPrice",
                table: "subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanName",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "PlanPrice",
                table: "subscriptions");
        }
    }
}

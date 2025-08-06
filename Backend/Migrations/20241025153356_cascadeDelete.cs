using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymSystem.Migrations
{
    /// <inheritdoc />
    public partial class cascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_attendances_Members",
                table: "attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_subscriptions_Members",
                table: "subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_subscriptions_plans",
                table: "subscriptions");

            migrationBuilder.AddForeignKey(
                name: "FK_attendances_Members",
                table: "attendances",
                column: "memberId",
                principalTable: "Members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptions_Members",
                table: "subscriptions",
                column: "memberId",
                principalTable: "Members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptions_plans",
                table: "subscriptions",
                column: "planId",
                principalTable: "plans",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_attendances_Members",
                table: "attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_subscriptions_Members",
                table: "subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_subscriptions_plans",
                table: "subscriptions");

            migrationBuilder.AddForeignKey(
                name: "FK_attendances_Members",
                table: "attendances",
                column: "memberId",
                principalTable: "Members",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptions_Members",
                table: "subscriptions",
                column: "memberId",
                principalTable: "Members",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptions_plans",
                table: "subscriptions",
                column: "planId",
                principalTable: "plans",
                principalColumn: "id");
        }
    }
}

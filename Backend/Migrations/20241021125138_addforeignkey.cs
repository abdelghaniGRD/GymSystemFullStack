using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymSystem.Migrations
{
    /// <inheritdoc />
    public partial class addforeignkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Members_AspNetUsers_aspNetUserId",
                table: "Members",
                column: "aspNetUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_AspNetUsers_aspNetUserId",
                table: "Members");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "UserInfos");

            migrationBuilder.CreateIndex(
                name: "IX_Slots_UserId",
                table: "Slots",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Slots_AspNetUsers_UserId",
                table: "Slots",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slots_AspNetUsers_UserId",
                table: "Slots");

            migrationBuilder.DropIndex(
                name: "IX_Slots_UserId",
                table: "Slots");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "UserInfos",
                nullable: true);
        }
    }
}

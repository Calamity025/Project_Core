using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class m2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserInfoId3",
                table: "Slots",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Slots_UserInfoId3",
                table: "Slots",
                column: "UserInfoId3");

            migrationBuilder.AddForeignKey(
                name: "FK_Slots_UserInfos_UserInfoId3",
                table: "Slots",
                column: "UserInfoId3",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slots_UserInfos_UserInfoId3",
                table: "Slots");

            migrationBuilder.DropIndex(
                name: "IX_Slots_UserInfoId3",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "UserInfoId3",
                table: "Slots");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BetHistories_UserInfos_UserInfoId",
                table: "BetHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Slots_AspNetUsers_UserId",
                table: "Slots");

            migrationBuilder.DropForeignKey(
                name: "FK_Slots_UserInfos_UserInfoId",
                table: "Slots");

            migrationBuilder.DropIndex(
                name: "IX_Slots_UserId",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Slots");

            migrationBuilder.RenameColumn(
                name: "UserInfoId",
                table: "BetHistories",
                newName: "BetUserInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_BetHistories_UserInfoId",
                table: "BetHistories",
                newName: "IX_BetHistories_BetUserInfoId");

            migrationBuilder.AlterColumn<int>(
                name: "UserInfoId",
                table: "Slots",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BetHistories_UserInfos_BetUserInfoId",
                table: "BetHistories",
                column: "BetUserInfoId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Slots_UserInfos_UserInfoId",
                table: "Slots",
                column: "UserInfoId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BetHistories_UserInfos_BetUserInfoId",
                table: "BetHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Slots_UserInfos_UserInfoId",
                table: "Slots");

            migrationBuilder.RenameColumn(
                name: "BetUserInfoId",
                table: "BetHistories",
                newName: "UserInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_BetHistories_BetUserInfoId",
                table: "BetHistories",
                newName: "IX_BetHistories_UserInfoId");

            migrationBuilder.AlterColumn<int>(
                name: "UserInfoId",
                table: "Slots",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Slots",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Slots_UserId",
                table: "Slots",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BetHistories_UserInfos_UserInfoId",
                table: "BetHistories",
                column: "UserInfoId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Slots_AspNetUsers_UserId",
                table: "Slots",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Slots_UserInfos_UserInfoId",
                table: "Slots",
                column: "UserInfoId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

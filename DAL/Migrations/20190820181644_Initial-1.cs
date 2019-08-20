using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FollowingSlots",
                columns: table => new
                {
                    FollowingUserInfoId = table.Column<int>(nullable: false),
                    SlotId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowingSlots", x => new { x.SlotId, x.FollowingUserInfoId });
                    table.ForeignKey(
                        name: "FK_FollowingSlots_UserInfos_FollowingUserInfoId",
                        column: x => x.FollowingUserInfoId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FollowingSlots_Slots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "Slots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowingSlots_FollowingUserInfoId",
                table: "FollowingSlots",
                column: "FollowingUserInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowingSlots");
        }
    }
}

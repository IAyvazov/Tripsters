using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tripsters.Data.Migrations
{
    public partial class AddUsersBadgesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserBadge");

            migrationBuilder.CreateTable(
                name: "UsersBadges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BadgeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersBadges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersBadges_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersBadges_Badges_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersBadges_BadgeId",
                table: "UsersBadges",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersBadges_IsDeleted",
                table: "UsersBadges",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UsersBadges_UserId",
                table: "UsersBadges",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersBadges");

            migrationBuilder.CreateTable(
                name: "ApplicationUserBadge",
                columns: table => new
                {
                    BadgesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserBadge", x => new { x.BadgesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserBadge_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUserBadge_Badges_BadgesId",
                        column: x => x.BadgesId,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserBadge_UsersId",
                table: "ApplicationUserBadge",
                column: "UsersId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Tripsters.Data.Migrations
{
    public partial class DeleteTripIdFromPhotoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Likes_LikeTripId_LikeUserId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LikeTripId_LikeUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "LikeTripId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "TripId",
                table: "Likes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes",
                table: "Likes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_TripId",
                table: "Likes",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LikeUserId",
                table: "AspNetUsers",
                column: "LikeUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Likes_LikeUserId",
                table: "AspNetUsers",
                column: "LikeUserId",
                principalTable: "Likes",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Likes_LikeUserId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_TripId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LikeUserId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "TripId",
                table: "Likes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Likes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LikeTripId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes",
                table: "Likes",
                columns: new[] { "TripId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LikeTripId_LikeUserId",
                table: "AspNetUsers",
                columns: new[] { "LikeTripId", "LikeUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Likes_LikeTripId_LikeUserId",
                table: "AspNetUsers",
                columns: new[] { "LikeTripId", "LikeUserId" },
                principalTable: "Likes",
                principalColumns: new[] { "TripId", "UserId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}

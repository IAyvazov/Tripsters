using Microsoft.EntityFrameworkCore.Migrations;

namespace Tripsters.Data.Migrations
{
    public partial class AddPhotoIdInLikeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "Likes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_PhotoId",
                table: "Likes",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Photo_PhotoId",
                table: "Likes",
                column: "PhotoId",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Photo_PhotoId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_PhotoId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Likes");
        }
    }
}

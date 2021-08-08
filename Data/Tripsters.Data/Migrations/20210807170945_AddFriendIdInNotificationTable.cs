namespace Tripsters.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddFriendIdInNotificationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FriendId",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FriendId",
                table: "Notifications");
        }
    }
}

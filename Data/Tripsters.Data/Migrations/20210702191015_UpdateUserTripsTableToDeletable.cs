using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tripsters.Data.Migrations
{
    public partial class UpdateUserTripsTableToDeletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "UserTrips",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "UserTrips",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "UserTrips",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserTrips",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "UserTrips",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTrips_IsDeleted",
                table: "UserTrips",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserTrips_IsDeleted",
                table: "UserTrips");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UserTrips");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UserTrips");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserTrips");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserTrips");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "UserTrips");
        }
    }
}

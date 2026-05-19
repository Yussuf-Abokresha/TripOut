using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Notifications.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotificationSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "messages",
                schema: "ntf",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "promotions",
                schema: "ntf",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "trip_updates",
                schema: "ntf",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "messages",
                schema: "ntf",
                table: "users");

            migrationBuilder.DropColumn(
                name: "promotions",
                schema: "ntf",
                table: "users");

            migrationBuilder.DropColumn(
                name: "trip_updates",
                schema: "ntf",
                table: "users");
        }
    }
}

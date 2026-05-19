using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesAndPagination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                schema: "usr",
                table: "users");

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "usr",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => new { x.user_id, x.role });
                    table.ForeignKey(
                        name: "fk_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "usr",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "usr");

            migrationBuilder.AddColumn<string>(
                name: "role",
                schema: "usr",
                table: "users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}

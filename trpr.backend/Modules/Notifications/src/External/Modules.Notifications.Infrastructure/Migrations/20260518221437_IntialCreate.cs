using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Modules.Notifications.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IntialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ntf");

            migrationBuilder.CreateTable(
                name: "inbox_consumer_messages",
                schema: "ntf",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    handler_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inbox_consumer_messages", x => new { x.id, x.handler_name });
                });

            migrationBuilder.CreateTable(
                name: "inbox_messages",
                schema: "ntf",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    correlation_id = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    occurred_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_consumer_messages",
                schema: "ntf",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    handler_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_consumer_messages", x => new { x.id, x.handler_name });
                });

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                schema: "ntf",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    correlation_id = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    occurred_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "ntf",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                schema: "ntf",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    content_type = table.Column<int>(type: "integer", nullable: false),
                    notify_email = table.Column<bool>(type: "boolean", nullable: false),
                    notify_phone = table.Column<bool>(type: "boolean", nullable: false),
                    notify_system = table.Column<bool>(type: "boolean", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_notifications_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "ntf",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "templates",
                schema: "ntf",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    content_type = table.Column<int>(type: "integer", nullable: false),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    template_type = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_templates", x => x.id);
                    table.ForeignKey(
                        name: "fk_templates_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "ntf",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "template_langs",
                schema: "ntf",
                columns: table => new
                {
                    template_id = table.Column<Guid>(type: "uuid", nullable: false),
                    lang_code = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_template_langs", x => new { x.template_id, x.lang_code });
                    table.ForeignKey(
                        name: "fk_template_langs_templates_template_id",
                        column: x => x.template_id,
                        principalSchema: "ntf",
                        principalTable: "templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "ntf",
                table: "templates",
                columns: new[] { "id", "active", "content_type", "created_at_utc", "template_type", "updated_at_utc", "user_id" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), true, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), true, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), true, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), true, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                schema: "ntf",
                table: "template_langs",
                columns: new[] { "lang_code", "template_id", "content", "created_at_utc", "title", "updated_at_utc" },
                values: new object[,]
                {
                    { "en", new Guid("11111111-1111-1111-1111-111111111111"), "Your request has been approved.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approval Message", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "en", new Guid("22222222-2222-2222-2222-222222222222"), "Your request has been rejected. Rejection reason: {{reason}}", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rejection Message", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "en", new Guid("33333333-3333-3333-3333-333333333333"), "Your OTP code is: {{code}}", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "OTP Message", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "en", new Guid("44444444-4444-4444-4444-444444444444"), "Your OTP code is: {{code}}", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Forget Password OTP Message", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "ix_notifications_user_id",
                schema: "ntf",
                table: "notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_templates_active",
                schema: "ntf",
                table: "templates",
                column: "active");

            migrationBuilder.CreateIndex(
                name: "ix_templates_user_id",
                schema: "ntf",
                table: "templates",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inbox_consumer_messages",
                schema: "ntf");

            migrationBuilder.DropTable(
                name: "inbox_messages",
                schema: "ntf");

            migrationBuilder.DropTable(
                name: "notifications",
                schema: "ntf");

            migrationBuilder.DropTable(
                name: "outbox_consumer_messages",
                schema: "ntf");

            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "ntf");

            migrationBuilder.DropTable(
                name: "template_langs",
                schema: "ntf");

            migrationBuilder.DropTable(
                name: "templates",
                schema: "ntf");

            migrationBuilder.DropTable(
                name: "users",
                schema: "ntf");
        }
    }
}

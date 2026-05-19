using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Conversations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IntialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cnv");

            migrationBuilder.CreateTable(
                name: "inbox_consumer_messages",
                schema: "cnv",
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
                schema: "cnv",
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
                schema: "cnv",
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
                schema: "cnv",
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
                schema: "cnv",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    identifier = table.Column<string>(type: "text", nullable: false),
                    avatar_url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ai_conversations",
                schema: "cnv",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ai_conversations", x => x.id);
                    table.ForeignKey(
                        name: "fk_ai_conversations_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "cnv",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "conversations",
                schema: "cnv",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    create_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    last_message_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_conversations", x => x.id);
                    table.ForeignKey(
                        name: "fk_conversations_users_create_by_user_id",
                        column: x => x.create_by_user_id,
                        principalSchema: "cnv",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ai_messages",
                schema: "cnv",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    parent_message_id = table.Column<string>(type: "text", nullable: true),
                    conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contnet = table.Column<string>(type: "text", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ai_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_ai_messages_ai_conversations_conversation_id",
                        column: x => x.conversation_id,
                        principalSchema: "cnv",
                        principalTable: "ai_conversations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ai_messages_ai_messages_parent_message_id",
                        column: x => x.parent_message_id,
                        principalSchema: "cnv",
                        principalTable: "ai_messages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "conversation_participants",
                schema: "cnv",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    joined_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_admin = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_conversation_participants", x => x.id);
                    table.ForeignKey(
                        name: "fk_conversation_participants_conversations_conversation_id",
                        column: x => x.conversation_id,
                        principalSchema: "cnv",
                        principalTable: "conversations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_conversation_participants_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "cnv",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                schema: "cnv",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sender_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    content = table.Column<string>(type: "text", nullable: false),
                    sent_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_messages_conversations_conversation_id",
                        column: x => x.conversation_id,
                        principalSchema: "cnv",
                        principalTable: "conversations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_messages_users_sender_user_id",
                        column: x => x.sender_user_id,
                        principalSchema: "cnv",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "message_attachments",
                schema: "cnv",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    attachment_name = table.Column<string>(type: "text", nullable: false),
                    attachment_type = table.Column<int>(type: "integer", nullable: false),
                    attachment_size = table.Column<long>(type: "bigint", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_message_attachments", x => x.id);
                    table.ForeignKey(
                        name: "fk_message_attachments_messages_message_id",
                        column: x => x.message_id,
                        principalSchema: "cnv",
                        principalTable: "messages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_ai_conversations_user_id",
                schema: "cnv",
                table: "ai_conversations",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_ai_messages_conversation_id",
                schema: "cnv",
                table: "ai_messages",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "ix_ai_messages_parent_message_id",
                schema: "cnv",
                table: "ai_messages",
                column: "parent_message_id");

            migrationBuilder.CreateIndex(
                name: "ix_conversation_participants_conversation_id",
                schema: "cnv",
                table: "conversation_participants",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "ix_conversation_participants_user_id",
                schema: "cnv",
                table: "conversation_participants",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_conversations_create_by_user_id",
                schema: "cnv",
                table: "conversations",
                column: "create_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_message_attachments_message_id",
                schema: "cnv",
                table: "message_attachments",
                column: "message_id");

            migrationBuilder.CreateIndex(
                name: "ix_messages_conversation_id",
                schema: "cnv",
                table: "messages",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "ix_messages_sender_user_id",
                schema: "cnv",
                table: "messages",
                column: "sender_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ai_messages",
                schema: "cnv");

            migrationBuilder.DropTable(
                name: "conversation_participants",
                schema: "cnv");

            migrationBuilder.DropTable(
                name: "inbox_consumer_messages",
                schema: "cnv");

            migrationBuilder.DropTable(
                name: "inbox_messages",
                schema: "cnv");

            migrationBuilder.DropTable(
                name: "message_attachments",
                schema: "cnv");

            migrationBuilder.DropTable(
                name: "outbox_consumer_messages",
                schema: "cnv");

            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "cnv");

            migrationBuilder.DropTable(
                name: "ai_conversations",
                schema: "cnv");

            migrationBuilder.DropTable(
                name: "messages",
                schema: "cnv");

            migrationBuilder.DropTable(
                name: "conversations",
                schema: "cnv");

            migrationBuilder.DropTable(
                name: "users",
                schema: "cnv");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Modules.Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IntialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "usr");

            migrationBuilder.CreateTable(
                name: "inbox_consumer_messages",
                schema: "usr",
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
                schema: "usr",
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
                name: "interests",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    icon = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_interests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "languages",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    native_name = table.Column<string>(type: "text", nullable: false),
                    icon = table.Column<string>(type: "text", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_languages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_consumer_messages",
                schema: "usr",
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
                schema: "usr",
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
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_verified = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vibes",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    thumbnail = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vibes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "guide_upgrade_request",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    rejection_reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    description = table.Column<string>(type: "text", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: true),
                    admin_id = table.Column<Guid>(type: "uuid", nullable: true),
                    reviewed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_guide_upgrade_request", x => x.id);
                    table.ForeignKey(
                        name: "fk_guide_upgrade_request_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "usr",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "profiles",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    bio = table.Column<string>(type: "text", nullable: false),
                    avatar_url = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_profiles", x => x.id);
                    table.ForeignKey(
                        name: "fk_profiles_users_id",
                        column: x => x.id,
                        principalSchema: "usr",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tokens",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_revoked = table.Column<bool>(type: "boolean", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_tokens_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "usr",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "document",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    guide_request_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    file_url = table.Column<string>(type: "text", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_document", x => x.id);
                    table.ForeignKey(
                        name: "fk_document_guide_upgrade_request_guide_request_id",
                        column: x => x.guide_request_id,
                        principalSchema: "usr",
                        principalTable: "guide_upgrade_request",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "profile_interests",
                schema: "usr",
                columns: table => new
                {
                    profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    interest_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_profile_interests", x => new { x.profile_id, x.interest_id });
                    table.ForeignKey(
                        name: "fk_profile_interests_interests_interest_id",
                        column: x => x.interest_id,
                        principalSchema: "usr",
                        principalTable: "interests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_profile_interests_profiles_profile_id",
                        column: x => x.profile_id,
                        principalSchema: "usr",
                        principalTable: "profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "profile_languages",
                schema: "usr",
                columns: table => new
                {
                    profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_profile_languages", x => new { x.profile_id, x.language_id });
                    table.ForeignKey(
                        name: "fk_profile_languages_languages_language_id",
                        column: x => x.language_id,
                        principalSchema: "usr",
                        principalTable: "languages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_profile_languages_profiles_profile_id",
                        column: x => x.profile_id,
                        principalSchema: "usr",
                        principalTable: "profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "profile_vibes",
                schema: "usr",
                columns: table => new
                {
                    profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    vibe_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_profile_vibes", x => new { x.profile_id, x.vibe_id });
                    table.ForeignKey(
                        name: "fk_profile_vibes_profiles_profile_id",
                        column: x => x.profile_id,
                        principalSchema: "usr",
                        principalTable: "profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_profile_vibes_vibes_vibe_id",
                        column: x => x.vibe_id,
                        principalSchema: "usr",
                        principalTable: "vibes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "usr",
                table: "interests",
                columns: new[] { "id", "created_at_utc", "icon", "name", "updated_at_utc" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/interests/history.png", "History", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/interests/adventure.png", "Adventure", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/interests/nature.png", "Nature", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/interests/culture.png", "Culture", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/interests/foodie.png", "Foodie", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/interests/wellness.png", "Wellness", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/interests/romantic.png", "Romantic", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                schema: "usr",
                table: "languages",
                columns: new[] { "id", "code", "created_at_utc", "icon", "name", "native_name", "updated_at_utc" },
                values: new object[,]
                {
                    { 1, "en", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/english.png", "English", "English", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "es", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/spanish.png", "Spanish", "Español", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "fr", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/french.png", "French", "Français", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "de", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/german.png", "German", "Deutsch", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "zh", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/chinese.png", "Chinese", "中文", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "ja", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/japanese.png", "Japanese", "日本語", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "ko", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/korean.png", "Korean", "한국어", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "pt", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/portuguese.png", "Portuguese", "Português", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "ru", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/russian.png", "Russian", "Русский", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "ar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/arabic.png", "Arabic", "العربية", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                schema: "usr",
                table: "vibes",
                columns: new[] { "id", "created_at_utc", "description", "name", "thumbnail", "updated_at_utc" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Self Discovery and Personal Growth.", "Solo", "@/assets/vibes/solo.png", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Night out with friends and socializing.", "Friends", "@/assets/vibes/friends.png", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Intimate and romantic setting.", "Romantic", "@/assets/vibes/romantic.png", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kid friendly and family-oriented atmosphere.", "Family", "@/assets/vibes/family.png", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "ix_document_guide_request_id",
                schema: "usr",
                table: "document",
                column: "guide_request_id");

            migrationBuilder.CreateIndex(
                name: "ix_guide_upgrade_request_user_id",
                schema: "usr",
                table: "guide_upgrade_request",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_profile_interests_interest_id",
                schema: "usr",
                table: "profile_interests",
                column: "interest_id");

            migrationBuilder.CreateIndex(
                name: "ix_profile_languages_language_id",
                schema: "usr",
                table: "profile_languages",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "ix_profile_vibes_vibe_id",
                schema: "usr",
                table: "profile_vibes",
                column: "vibe_id");

            migrationBuilder.CreateIndex(
                name: "ix_tokens_user_id",
                schema: "usr",
                table: "tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_user_name",
                schema: "usr",
                table: "users",
                column: "user_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "document",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "inbox_consumer_messages",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "inbox_messages",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "outbox_consumer_messages",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "profile_interests",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "profile_languages",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "profile_vibes",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "tokens",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "guide_upgrade_request",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "interests",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "languages",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "profiles",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "vibes",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "users",
                schema: "usr");
        }
    }
}

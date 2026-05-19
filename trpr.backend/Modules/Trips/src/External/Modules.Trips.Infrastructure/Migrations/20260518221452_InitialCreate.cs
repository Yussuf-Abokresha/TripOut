using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "trp");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "csv_seed_histories",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    seeded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csv_seed_histories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "governorates",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_governorates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "inbox_consumer_messages",
                schema: "trp",
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
                schema: "trp",
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
                schema: "trp",
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
                schema: "trp",
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
                name: "tags",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "themes",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_themes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "trp",
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
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "places",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    rating = table.Column<double>(type: "double precision", nullable: true),
                    average_visit_time = table.Column<double>(type: "double precision", nullable: true),
                    osrm_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    visit_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    rate_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    governorate_id = table.Column<int>(type: "integer", nullable: false),
                    location = table.Column<Point>(type: "geography (Point, 4326)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_places", x => x.id);
                    table.ForeignKey(
                        name: "fk_places_categories_category_id",
                        column: x => x.category_id,
                        principalSchema: "trp",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_places_governorates_governorate_id",
                        column: x => x.governorate_id,
                        principalSchema: "trp",
                        principalTable: "governorates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "theme_categories",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    theme_id = table.Column<int>(type: "integer", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    max_limit = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_theme_categories", x => x.id);
                    table.ForeignKey(
                        name: "fk_theme_categories_categories_category_id",
                        column: x => x.category_id,
                        principalSchema: "trp",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_theme_categories_themes_theme_id",
                        column: x => x.theme_id,
                        principalSchema: "trp",
                        principalTable: "themes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "theme_tags",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    theme_id = table.Column<int>(type: "integer", nullable: false),
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    score = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_theme_tags", x => x.id);
                    table.ForeignKey(
                        name: "fk_theme_tags_tags_tag_id",
                        column: x => x.tag_id,
                        principalSchema: "trp",
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_theme_tags_themes_theme_id",
                        column: x => x.theme_id,
                        principalSchema: "trp",
                        principalTable: "themes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trip",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    theme_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<double>(type: "double precision", precision: 10, scale: 2, nullable: false),
                    actual_duration = table.Column<double>(type: "double precision", nullable: false),
                    expected_duration = table.Column<double>(type: "double precision", nullable: false),
                    images = table.Column<string[]>(type: "text[]", nullable: false),
                    trip_visibility = table.Column<int>(type: "integer", nullable: false),
                    max_participants_count = table.Column<int>(type: "integer", nullable: false),
                    guide_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    rejection_reason = table.Column<string>(type: "text", nullable: true),
                    publish_mode = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip", x => x.id);
                    table.ForeignKey(
                        name: "fk_trip_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "trp",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "place_tags",
                schema: "trp",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    place_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_place_tags", x => new { x.place_id, x.tag_id });
                    table.ForeignKey(
                        name: "fk_place_tags_places_place_id",
                        column: x => x.place_id,
                        principalSchema: "trp",
                        principalTable: "places",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_place_tags_tags_tag_id",
                        column: x => x.tag_id,
                        principalSchema: "trp",
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "day",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    trip_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_day", x => x.id);
                    table.ForeignKey(
                        name: "fk_day_trip_trip_id",
                        column: x => x.trip_id,
                        principalSchema: "trp",
                        principalTable: "trip",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trip_governorate",
                schema: "trp",
                columns: table => new
                {
                    trip_id = table.Column<Guid>(type: "uuid", nullable: false),
                    governorate_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_governorate", x => new { x.trip_id, x.governorate_id });
                    table.ForeignKey(
                        name: "fk_trip_governorate_governorates_governorate_id",
                        column: x => x.governorate_id,
                        principalSchema: "trp",
                        principalTable: "governorates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_trip_governorate_trip_trip_id",
                        column: x => x.trip_id,
                        principalSchema: "trp",
                        principalTable: "trip",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trip_participant",
                schema: "trp",
                columns: table => new
                {
                    trip_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_participant", x => new { x.trip_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_trip_participant_trip_trip_id",
                        column: x => x.trip_id,
                        principalSchema: "trp",
                        principalTable: "trip",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_trip_participant_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "trp",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "place_day",
                schema: "trp",
                columns: table => new
                {
                    days_id = table.Column<Guid>(type: "uuid", nullable: false),
                    places_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_place_day", x => new { x.days_id, x.places_id });
                    table.ForeignKey(
                        name: "fk_place_day_day_days_id",
                        column: x => x.days_id,
                        principalSchema: "trp",
                        principalTable: "day",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_place_day_places_places_id",
                        column: x => x.places_id,
                        principalSchema: "trp",
                        principalTable: "places",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "trp",
                table: "categories",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "food_drink" },
                    { 2, "entertainment" },
                    { 3, "culture" },
                    { 4, "activities" },
                    { 5, "outdoor" },
                    { 6, "relaxation" },
                    { 7, "other" }
                });

            migrationBuilder.InsertData(
                schema: "trp",
                table: "governorates",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Aswan" },
                    { 2, "Asyut" },
                    { 3, "Luxor" },
                    { 4, "Alexandria" },
                    { 5, "Ismailia" },
                    { 6, "Suez" },
                    { 7, "Dakahlia" },
                    { 8, "Faiyum" },
                    { 9, "Cairo" },
                    { 10, "Giza" },
                    { 11, "Beheira" },
                    { 12, "Sharqia" },
                    { 13, "Gharbia" },
                    { 14, "Qalyubia" },
                    { 15, "Monufia" },
                    { 16, "Minya" },
                    { 17, "New Valley" },
                    { 18, "Beni Suef" },
                    { 19, "Port Said" },
                    { 20, "South Sinai" },
                    { 21, "Damietta" },
                    { 22, "Sohag" },
                    { 23, "North Sinai" },
                    { 24, "Qena" },
                    { 25, "Kafr El Sheikh" },
                    { 26, "Matruh" },
                    { 27, "Red Sea" }
                });

            migrationBuilder.InsertData(
                schema: "trp",
                table: "tags",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "cinema" },
                    { 2, "theatre" },
                    { 3, "live_shows" },
                    { 4, "nightlife" },
                    { 5, "concert_venue" },
                    { 6, "festival" },
                    { 7, "museum" },
                    { 8, "historical_site" },
                    { 9, "landmark" },
                    { 10, "religious_site" },
                    { 11, "art_gallery" },
                    { 12, "restaurant" },
                    { 13, "cafe" },
                    { 14, "street_food" },
                    { 15, "fine_dining" },
                    { 16, "fast_food" },
                    { 17, "rooftop" },
                    { 18, "family_friendly" },
                    { 19, "kid_friendly" },
                    { 20, "group_activities" },
                    { 21, "guided_tours" },
                    { 22, "adventure" },
                    { 23, "nature" },
                    { 24, "park" },
                    { 25, "beach" },
                    { 26, "desert" },
                    { 27, "river_view" },
                    { 28, "spa" },
                    { 29, "quiet_place" },
                    { 30, "luxury" },
                    { 31, "scenic" },
                    { 32, "romantic" },
                    { 33, "budget_friendly" },
                    { 34, "tourist_hotspot" },
                    { 35, "local_favorite" },
                    { 36, "instagrammable" },
                    { 37, "hidden_gem" },
                    { 38, "accessible" }
                });

            migrationBuilder.InsertData(
                schema: "trp",
                table: "themes",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "History" },
                    { 2, "Adventure" },
                    { 3, "Nature" },
                    { 4, "Culture" },
                    { 5, "Foodie" },
                    { 6, "Wellness" },
                    { 7, "Romantic" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_categories_name",
                schema: "trp",
                table: "categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_day_trip_id",
                schema: "trp",
                table: "day",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "ix_governorates_name",
                schema: "trp",
                table: "governorates",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_place_day_places_id",
                schema: "trp",
                table: "place_day",
                column: "places_id");

            migrationBuilder.CreateIndex(
                name: "ix_place_tags_tag_id",
                schema: "trp",
                table: "place_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "ix_places_category_id",
                schema: "trp",
                table: "places",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_places_governorate_id",
                schema: "trp",
                table: "places",
                column: "governorate_id");

            migrationBuilder.CreateIndex(
                name: "ix_places_location",
                schema: "trp",
                table: "places",
                column: "location")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "ix_tags_name",
                schema: "trp",
                table: "tags",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_theme_categories_category_id",
                schema: "trp",
                table: "theme_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_theme_categories_theme_id",
                schema: "trp",
                table: "theme_categories",
                column: "theme_id");

            migrationBuilder.CreateIndex(
                name: "ix_theme_tags_tag_id",
                schema: "trp",
                table: "theme_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "ix_theme_tags_theme_id",
                schema: "trp",
                table: "theme_tags",
                column: "theme_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_price",
                schema: "trp",
                table: "trip",
                column: "price");

            migrationBuilder.CreateIndex(
                name: "ix_trip_theme_id",
                schema: "trp",
                table: "trip",
                column: "theme_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_title",
                schema: "trp",
                table: "trip",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "ix_trip_user_id",
                schema: "trp",
                table: "trip",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_governorate_governorate_id",
                schema: "trp",
                table: "trip_governorate",
                column: "governorate_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_participant_user_id",
                schema: "trp",
                table: "trip_participant",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "csv_seed_histories",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "inbox_consumer_messages",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "inbox_messages",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "outbox_consumer_messages",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "place_day",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "place_tags",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "theme_categories",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "theme_tags",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "trip_governorate",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "trip_participant",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "day",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "places",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "tags",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "themes",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "trip",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "governorates",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "user",
                schema: "trp");
        }
    }
}

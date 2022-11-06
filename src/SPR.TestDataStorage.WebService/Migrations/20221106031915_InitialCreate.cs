using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPR.TestDataStorage.WebService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "data");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pgcrypto", ",,");

            migrationBuilder.CreateTable(
                name: "data_section",
                schema: "data",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "public.gen_random_uuid()"),
                    system_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_section", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "object_type",
                schema: "data",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "public.gen_random_uuid()"),
                    system_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_object_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "project",
                schema: "data",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "public.gen_random_uuid()"),
                    system_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "object_type_data_section",
                schema: "data",
                columns: table => new
                {
                    object_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    data_section_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "object_type_data_section_data_section_id_fkey",
                        column: x => x.data_section_id,
                        principalSchema: "data",
                        principalTable: "data_section",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "object_type_data_section_object_type_id_fkey",
                        column: x => x.object_type_id,
                        principalSchema: "data",
                        principalTable: "object_type",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "data_header",
                schema: "data",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "public.gen_random_uuid()"),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    object_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    object_identity = table.Column<string>(type: "text", nullable: false),
                    data_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_header", x => x.id);
                    table.ForeignKey(
                        name: "data_header_object_type_id_fkey",
                        column: x => x.object_type_id,
                        principalSchema: "data",
                        principalTable: "object_type",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "data_header_project_id_fkey",
                        column: x => x.project_id,
                        principalSchema: "data",
                        principalTable: "project",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "data_content",
                schema: "data",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "public.gen_random_uuid()"),
                    data_header_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_sections = table.Column<string>(type: "json", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_content", x => x.id);
                    table.ForeignKey(
                        name: "data_content_data_header_id_fkey",
                        column: x => x.data_header_id,
                        principalSchema: "data",
                        principalTable: "data_header",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "data_content_data_header_id_hash_key",
                schema: "data",
                table: "data_content",
                columns: new[] { "data_header_id", "hash" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "data_header_project_id_object_type_id_object_identity_data__key",
                schema: "data",
                table: "data_header",
                columns: new[] { "project_id", "object_type_id", "object_identity", "data_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_data_header_object_type_id",
                schema: "data",
                table: "data_header",
                column: "object_type_id");

            migrationBuilder.CreateIndex(
                name: "data_section_system_name_key",
                schema: "data",
                table: "data_section",
                column: "system_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "object_type_system_name_key",
                schema: "data",
                table: "object_type",
                column: "system_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_object_type_data_section_data_section_id",
                schema: "data",
                table: "object_type_data_section",
                column: "data_section_id");

            migrationBuilder.CreateIndex(
                name: "object_type_data_section_object_type_id_data_section_id_key",
                schema: "data",
                table: "object_type_data_section",
                columns: new[] { "object_type_id", "data_section_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "project_system_name_key",
                schema: "data",
                table: "project",
                column: "system_name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "data_content",
                schema: "data");

            migrationBuilder.DropTable(
                name: "object_type_data_section",
                schema: "data");

            migrationBuilder.DropTable(
                name: "data_header",
                schema: "data");

            migrationBuilder.DropTable(
                name: "data_section",
                schema: "data");

            migrationBuilder.DropTable(
                name: "object_type",
                schema: "data");

            migrationBuilder.DropTable(
                name: "project",
                schema: "data");
        }
    }
}

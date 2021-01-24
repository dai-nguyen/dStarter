using System;
using System.Collections.Generic;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Migrations
{
    public partial class addwiki : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wikis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    Tags = table.Column<string[]>(type: "text[]", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, defaultValue: "?"),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, defaultValue: "?"),
                    ExternalId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CustomAttributes = table.Column<ICollection<CustomAttribute>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wikis", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wikis_ExternalId",
                table: "Wikis",
                column: "ExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_Wikis_Tags",
                table: "Wikis",
                column: "Tags");

            migrationBuilder.CreateIndex(
                name: "IX_Wikis_Title_Body",
                table: "Wikis",
                columns: new[] { "Title", "Body" })
                .Annotation("Npgsql:TsVectorConfig", "english");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wikis");
        }
    }
}

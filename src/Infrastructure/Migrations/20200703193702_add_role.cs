using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class add_role : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "BgSchedules");

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "BgSchedules",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RoleAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true, defaultValue: "?"),
                    UpdatedBy = table.Column<string>(maxLength: 100, nullable: true, defaultValue: "?"),
                    ExternalId = table.Column<string>(maxLength: 100, nullable: true),
                    RoleId = table.Column<string>(maxLength: 100, nullable: true),
                    Type = table.Column<string>(maxLength: 50, nullable: true),
                    Value = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleAttributes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleAttributes_ExternalId",
                table: "RoleAttributes",
                column: "ExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAttributes_RoleId_Type",
                table: "RoleAttributes",
                columns: new[] { "RoleId", "Type" },
                unique: true,
                filter: "[RoleId] IS NOT NULL AND [Type] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleAttributes");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "BgSchedules");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "BgSchedules",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class add_cust_attr_def : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomAttributes_ParentName_ParentId_Key",
                table: "CustomAttributes");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "CustomAttributes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CustomAttributes");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "CustomAttributes");

            migrationBuilder.DropColumn(
                name: "ParentName",
                table: "CustomAttributes");

            migrationBuilder.AddColumn<int>(
                name: "DefinitionId",
                table: "CustomAttributes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CustomAttributeDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true, defaultValue: "?"),
                    UpdatedBy = table.Column<string>(maxLength: 100, nullable: true, defaultValue: "?"),
                    ExternalId = table.Column<string>(maxLength: 100, nullable: true),
                    ObjectName = table.Column<string>(maxLength: 100, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: true),
                    DataType = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomAttributeDefinitions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomAttributes_DefinitionId",
                table: "CustomAttributes",
                column: "DefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomAttributeDefinitions_ExternalId",
                table: "CustomAttributeDefinitions",
                column: "ExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomAttributeDefinitions_ObjectName_Name",
                table: "CustomAttributeDefinitions",
                columns: new[] { "ObjectName", "Name" });

            migrationBuilder.AddForeignKey(
                name: "FK_CustomAttributes_CustomAttributeDefinitions_DefinitionId",
                table: "CustomAttributes",
                column: "DefinitionId",
                principalTable: "CustomAttributeDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomAttributes_CustomAttributeDefinitions_DefinitionId",
                table: "CustomAttributes");

            migrationBuilder.DropTable(
                name: "CustomAttributeDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_CustomAttributes_DefinitionId",
                table: "CustomAttributes");

            migrationBuilder.DropColumn(
                name: "DefinitionId",
                table: "CustomAttributes");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "CustomAttributes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CustomAttributes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "CustomAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ParentName",
                table: "CustomAttributes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomAttributes_ParentName_ParentId_Key",
                table: "CustomAttributes",
                columns: new[] { "ParentName", "ParentId", "Key" },
                unique: true,
                filter: "[ParentName] IS NOT NULL AND [Key] IS NOT NULL");
        }
    }
}

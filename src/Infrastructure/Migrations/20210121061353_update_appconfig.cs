using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class update_appconfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AppConfigs");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AppConfigs",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_raise_date",
                table: "Logs",
                column: "raise_date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Logs_raise_date",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AppConfigs");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AppConfigs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}

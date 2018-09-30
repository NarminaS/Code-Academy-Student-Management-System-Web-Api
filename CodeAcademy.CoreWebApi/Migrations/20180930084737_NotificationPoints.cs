using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeAcademy.CoreWebApi.Migrations
{
    public partial class NotificationPoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Notifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Notifications");
        }
    }
}

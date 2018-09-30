using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeAcademy.CoreWebApi.Migrations
{
    public partial class NotificationIsVisited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVisited",
                table: "Notifications",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisited",
                table: "Notifications");
        }
    }
}

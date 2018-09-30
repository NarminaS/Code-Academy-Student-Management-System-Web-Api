using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeAcademy.CoreWebApi.Migrations
{
    public partial class NotificationAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Notifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "Notifications");
        }
    }
}

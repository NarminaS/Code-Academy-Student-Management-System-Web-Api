using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeAcademy.CoreWebApi.Migrations
{
    public partial class NotificationTotalPoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Points",
                table: "Notifications",
                newName: "TotalPoints");

            migrationBuilder.AddColumn<int>(
                name: "Point",
                table: "Notifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Point",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "TotalPoints",
                table: "Notifications",
                newName: "Points");
        }
    }
}

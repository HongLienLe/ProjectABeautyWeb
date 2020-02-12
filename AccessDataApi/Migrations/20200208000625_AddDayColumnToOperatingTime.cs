using Microsoft.EntityFrameworkCore.Migrations;

namespace AccessDataApi.Migrations
{
    public partial class AddDayColumnToOperatingTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "OperatingTimes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "OperatingTimes");
        }
    }
}

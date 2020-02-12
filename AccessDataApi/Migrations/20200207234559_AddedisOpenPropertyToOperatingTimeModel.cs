using Microsoft.EntityFrameworkCore.Migrations;

namespace AccessDataApi.Migrations
{
    public partial class AddedisOpenPropertyToOperatingTimeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isOpen",
                table: "OperatingTimes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isOpen",
                table: "OperatingTimes");
        }
    }
}

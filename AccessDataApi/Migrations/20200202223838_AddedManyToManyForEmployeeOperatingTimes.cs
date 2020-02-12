using Microsoft.EntityFrameworkCore.Migrations;

namespace AccessDataApi.Migrations
{
    public partial class AddedManyToManyForEmployeeOperatingTimes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "OperatingTimes");

            migrationBuilder.CreateTable(
                name: "workSchedules",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(nullable: false),
                    OperatingTimeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workSchedules", x => new { x.EmployeeId, x.OperatingTimeId });
                    table.ForeignKey(
                        name: "FK_workSchedules_OperatingTimes_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "OperatingTimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_workSchedules_Employees_OperatingTimeId",
                        column: x => x.OperatingTimeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_workSchedules_OperatingTimeId",
                table: "workSchedules",
                column: "OperatingTimeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "workSchedules");

            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "OperatingTimes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

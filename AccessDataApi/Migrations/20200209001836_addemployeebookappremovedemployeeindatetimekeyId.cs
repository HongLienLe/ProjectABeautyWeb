using Microsoft.EntityFrameworkCore.Migrations;

namespace AccessDataApi.Migrations
{
    public partial class addemployeebookappremovedemployeeindatetimekeyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DateTimeKeys_Employees_EmployeeId",
                table: "DateTimeKeys");

            migrationBuilder.DropIndex(
                name: "IX_DateTimeKeys_EmployeeId",
                table: "DateTimeKeys");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "DateTimeKeys");

            migrationBuilder.AddColumn<int>(
                name: "Employee_Id",
                table: "BookApps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookApps_Employee_Id",
                table: "BookApps",
                column: "Employee_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookApps_Employees_Employee_Id",
                table: "BookApps",
                column: "Employee_Id",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookApps_Employees_Employee_Id",
                table: "BookApps");

            migrationBuilder.DropIndex(
                name: "IX_BookApps_Employee_Id",
                table: "BookApps");

            migrationBuilder.DropColumn(
                name: "Employee_Id",
                table: "BookApps");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "DateTimeKeys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DateTimeKeys_EmployeeId",
                table: "DateTimeKeys",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DateTimeKeys_Employees_EmployeeId",
                table: "DateTimeKeys",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

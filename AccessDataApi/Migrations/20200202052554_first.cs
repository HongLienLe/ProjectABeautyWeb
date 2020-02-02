using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccessDataApi.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientAccounts",
                columns: table => new
                {
                    ClientAccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    ContactNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAccounts", x => x.ClientAccountId);
                });

            migrationBuilder.CreateTable(
                name: "DateTimeKeys",
                columns: table => new
                {
                    DateTimeKeyId = table.Column<string>(nullable: false),
                    date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateTimeKeys", x => x.DateTimeKeyId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    TreatmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreatmentType = table.Column<int>(nullable: false),
                    TreatmentName = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Duration = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.TreatmentId);
                });

            migrationBuilder.CreateTable(
                name: "BookApps",
                columns: table => new
                {
                    BookAppId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Client_Id = table.Column<int>(nullable: false),
                    DateTimeKeyId = table.Column<string>(nullable: true),
                    Employee_Id = table.Column<int>(nullable: false),
                    Treatment_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookApps", x => x.BookAppId);
                    table.ForeignKey(
                        name: "FK_BookApps_ClientAccounts_Client_Id",
                        column: x => x.Client_Id,
                        principalTable: "ClientAccounts",
                        principalColumn: "ClientAccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookApps_DateTimeKeys_DateTimeKeyId",
                        column: x => x.DateTimeKeyId,
                        principalTable: "DateTimeKeys",
                        principalColumn: "DateTimeKeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookApps_Employees_Employee_Id",
                        column: x => x.Employee_Id,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookApps_Treatments_Treatment_Id",
                        column: x => x.Treatment_Id,
                        principalTable: "Treatments",
                        principalColumn: "TreatmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTreatment",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(nullable: false),
                    TreatmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTreatment", x => new { x.TreatmentId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_EmployeeTreatment_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeTreatment_Treatments_TreatmentId",
                        column: x => x.TreatmentId,
                        principalTable: "Treatments",
                        principalColumn: "TreatmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    BookAppId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_Reservations_BookApps_BookAppId",
                        column: x => x.BookAppId,
                        principalTable: "BookApps",
                        principalColumn: "BookAppId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookApps_Client_Id",
                table: "BookApps",
                column: "Client_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookApps_DateTimeKeyId",
                table: "BookApps",
                column: "DateTimeKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_BookApps_Employee_Id",
                table: "BookApps",
                column: "Employee_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookApps_Treatment_Id",
                table: "BookApps",
                column: "Treatment_Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTreatment_EmployeeId",
                table: "EmployeeTreatment",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_BookAppId",
                table: "Reservations",
                column: "BookAppId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeTreatment");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "BookApps");

            migrationBuilder.DropTable(
                name: "ClientAccounts");

            migrationBuilder.DropTable(
                name: "DateTimeKeys");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Treatments");
        }
    }
}

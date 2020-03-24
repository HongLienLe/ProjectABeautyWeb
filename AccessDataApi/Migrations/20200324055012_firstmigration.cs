using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccessDataApi.Migrations
{
    public partial class firstmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientAccounts",
                columns: table => new
                {
                    ClientAccountId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: false)
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
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "OperatingTimes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Day = table.Column<string>(nullable: true),
                    StartTime = table.Column<TimeSpan>(nullable: false),
                    EndTime = table.Column<TimeSpan>(nullable: false),
                    isOpen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatingTimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    TreatmentId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TreatmentType = table.Column<int>(nullable: false),
                    TreatmentName = table.Column<string>(nullable: false),
                    isAddOn = table.Column<bool>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Duration = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.TreatmentId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

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
                        name: "FK_workSchedules_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_workSchedules_OperatingTimes_OperatingTimeId",
                        column: x => x.OperatingTimeId,
                        principalTable: "OperatingTimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentDetails",
                columns: table => new
                {
                    AppointmentDetailsId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Client_Id = table.Column<int>(nullable: false),
                    DateTimeAppointment_Id = table.Column<string>(nullable: true),
                    Employee_Id = table.Column<int>(nullable: false),
                    Treatment_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentDetails", x => x.AppointmentDetailsId);
                    table.ForeignKey(
                        name: "FK_AppointmentDetails_ClientAccounts_Client_Id",
                        column: x => x.Client_Id,
                        principalTable: "ClientAccounts",
                        principalColumn: "ClientAccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentDetails_DateTimeKeys_DateTimeAppointment_Id",
                        column: x => x.DateTimeAppointment_Id,
                        principalTable: "DateTimeKeys",
                        principalColumn: "DateTimeKeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentDetails_Employees_Employee_Id",
                        column: x => x.Employee_Id,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentDetails_Treatments_Treatment_Id",
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
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    AppointmentDetailsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_Reservations_AppointmentDetails_AppointmentDetailsId",
                        column: x => x.AppointmentDetailsId,
                        principalTable: "AppointmentDetails",
                        principalColumn: "AppointmentDetailsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetails_Client_Id",
                table: "AppointmentDetails",
                column: "Client_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetails_DateTimeAppointment_Id",
                table: "AppointmentDetails",
                column: "DateTimeAppointment_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetails_Employee_Id",
                table: "AppointmentDetails",
                column: "Employee_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetails_Treatment_Id",
                table: "AppointmentDetails",
                column: "Treatment_Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTreatment_EmployeeId",
                table: "EmployeeTreatment",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_AppointmentDetailsId",
                table: "Reservations",
                column: "AppointmentDetailsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_workSchedules_OperatingTimeId",
                table: "workSchedules",
                column: "OperatingTimeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeTreatment");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "workSchedules");

            migrationBuilder.DropTable(
                name: "AppointmentDetails");

            migrationBuilder.DropTable(
                name: "OperatingTimes");

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

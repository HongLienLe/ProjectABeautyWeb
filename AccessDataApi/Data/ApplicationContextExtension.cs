using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Models;
using Itenso.TimePeriod;

namespace AccessDataApi.Data
{
    public static class ApplicationContextExtension
    {
        public static void CreateSeedData(this ApplicationContext context)
        {
            context.SaveChanges();

            if (context.Employees.Any())
                return;

                List<OperatingTime> operatingTimes = new List<OperatingTime>()
            {
                new OperatingTime(){Id = 1, Day = "Monday", StartTime = new TimeSpan(10,0,0), EndTime = new TimeSpan(19,0,0), isOpen = true },
                new OperatingTime(){Id = 2, Day = "Tuesday", StartTime = new TimeSpan(10,0,0), EndTime = new TimeSpan(19,0,0), isOpen = true },
                new OperatingTime(){Id = 3, Day = "Wednesday", StartTime = new TimeSpan(10,0,0), EndTime = new TimeSpan(19,0,0), isOpen = true },
                new OperatingTime(){Id = 4, Day = "Thursday", StartTime = new TimeSpan(10,0,0), EndTime = new TimeSpan(19,0,0), isOpen = true },
                new OperatingTime(){Id = 5, Day = "Friday", StartTime = new TimeSpan(10,0,0), EndTime = new TimeSpan(19,0,0), isOpen = true },
                new OperatingTime(){Id = 6, Day = "Saturday", StartTime = new TimeSpan(10,0,0), EndTime = new TimeSpan(19,0,0), isOpen = true },
                new OperatingTime(){Id = 7, Day = "Sunday", StartTime = new TimeSpan(0), EndTime = new TimeSpan(0), isOpen = false }
            };

                List<Employee> employees = new List<Employee>()
            {
                new Employee(){EmployeName = "Employee1", Email = "Employee1@gmail.com"},
                new Employee(){EmployeName = "Employee2", Email = "Employee1@gmail.com"},
                new Employee(){EmployeName = "Employee3",Email = "Employee1@gmail.com"}
            };

                List<Treatment> treatments = new List<Treatment>()
            {
              new Treatment() { TreatmentName = "Infill", TreatmentType = TreatmentType.Acrylic, Duration = 45, Price = 16 },
              new Treatment() { TreatmentName = "Fullset", TreatmentType = TreatmentType.Sns, Duration = 45, Price = 28 },
              new Treatment() { TreatmentName = "Infill", TreatmentType = TreatmentType.Sns, Duration = 45, Price = 25 },
              new Treatment() { TreatmentName = "Gel Polish Express", TreatmentType = TreatmentType.GelPolish, Duration = 20, Price = 20 },
              new Treatment() { TreatmentName = "Gel Polish with Manicure", TreatmentType = TreatmentType.GelPolish, Duration = 45, Price = 25 },
              new Treatment() { TreatmentName = "Pedicure", TreatmentType = TreatmentType.NaturalNail, Duration = 45, Price = 22 }
            };


            context.Employees.AddRange(employees);
            context.Treatments.AddRange(treatments);
            context.OperatingTimes.AddRange(operatingTimes);
            context.SaveChanges();

            List<EmployeeTreatment> employeeTreatments = new List<EmployeeTreatment>();

            for (int i = 1; i <= 3; i++)
            {
                var employee = context.Employees.Single(x => x.EmployeeId == i);
                var treatment = context.Treatments.ToList();
                employeeTreatments.AddRange(

                new List<EmployeeTreatment>() {
                    new EmployeeTreatment()
                    {
                        Employee = employee,
                        Treatment = treatment[0]
                    },
                    new EmployeeTreatment()
                    {
                        Employee = employee,
                        Treatment = treatment[1]
                    },
                    new EmployeeTreatment()
                    {
                        Employee = employee,
                        Treatment = treatment[2]
                    },
                    new EmployeeTreatment()
                    {
                        Employee = employee,
                        Treatment = treatment[3]
                    },
                    new EmployeeTreatment()
                    {
                        Employee = employee,
                        Treatment = treatment[4]
                    }
                    }
                );

                if (i == 3)
                {
                    employeeTreatments.Add(
                        new EmployeeTreatment()
                        {
                            Employee = employee,
                            Treatment = treatment[5]
                        });
                }

            }

            context.EmployeeTreatment.AddRange(employeeTreatments);
            context.SaveChanges();

                List<OperatingTimeEmployee> operatingTimeEmployees = new List<OperatingTimeEmployee>();

                for (int i = 1; i <= 3; i++)
                {
                    var employee = context.Employees.Single(x => x.EmployeeId == i);
                    var operatingTime = context.OperatingTimes.ToList();

                    if (i != 3)
                    {
                        operatingTimeEmployees.AddRange(
                            new List<OperatingTimeEmployee>() {
                            new OperatingTimeEmployee()
                            {
                                Employee = employee,
                                OperatingTime = operatingTimes[0]
                            },
                            new OperatingTimeEmployee()
                            {
                                Employee = employee,
                                OperatingTime = operatingTimes[1]
                            },
                            new OperatingTimeEmployee()
                            {
                                Employee = employee,
                                OperatingTime = operatingTimes[2]
                            },
                            new OperatingTimeEmployee()
                            {
                                Employee = employee,
                                OperatingTime = operatingTimes[3]
                            }

                            });

                    }

                    operatingTimeEmployees.AddRange(
                        new List<OperatingTimeEmployee>()
                        {
                         new OperatingTimeEmployee()
                            {
                                Employee = employee,
                                OperatingTime = operatingTimes[4]
                            },
                            new OperatingTimeEmployee()
                            {
                                Employee = employee,
                                OperatingTime = operatingTimes[5]
                            }
                        });
                }

            context.workSchedules.AddRange(operatingTimeEmployees);
            context.SaveChanges();

            var date = new DateTime(2020, 3, 9);

            var datetimekey = new DateTimeKey()
            {
                DateTimeKeyId = date.ToShortDateString(),
                date = date
            };

            var requestedBookApp = new AppointmentDetails()
            {
                ClientAccount = new ClientAccount() { FirstName = "Test", Email = "Fake@gmail.com", ContactNumber = "12345678901" },
                TreatmentId = 1,
                EmployeeId = 1,
                DateTimeKey = datetimekey,
                Reservation = new Reservation() { StartTime = date.AddHours(10), EndTime = date.AddHours(10).AddMinutes(45) }
            };

            context.AppointmentDetails.Add(requestedBookApp);
            context.SaveChanges();


        }
        
    }
}


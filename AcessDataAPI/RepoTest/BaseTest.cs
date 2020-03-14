﻿using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.Models;

namespace AcessDataAPITest.RepoTest
{
    public class BaseTest
    {
        public ConnectionFactory _connectionFactory;
        public ApplicationContext _context;

        public void EndConnection()
        {
            _context.Database.EnsureDeleted();
            _connectionFactory.connection.Close();

        }

        public List<OperatingTime> GetOpeningTimes()
        {
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

            return operatingTimes;
        }

        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>()
            {
                new Employee(){EmployeName = "Employee1", Email = "Employee1@gmail.com"},
                new Employee(){EmployeName = "Employee2", Email = "Employee1@gmail.com"},
                new Employee(){EmployeName = "Employee3",Email = "Employee1@gmail.com"}
            };

            return employees;
        }

        public List<Treatment> GetTreatments()
        {
            List<Treatment> treatments = new List<Treatment>()
            {
              new Treatment() { TreatmentName = "Infill", TreatmentType = TreatmentType.Acrylic, Duration = 45, Price = 16 },
              new Treatment() { TreatmentName = "Fullset", TreatmentType = TreatmentType.Sns, Duration = 45, Price = 28 },
              new Treatment() { TreatmentName = "Infill", TreatmentType = TreatmentType.Sns, Duration = 45, Price = 25 },
              new Treatment() { TreatmentName = "Gel Polish Express", TreatmentType = TreatmentType.GelPolish, Duration = 20, Price = 20 },
              new Treatment() { TreatmentName = "Gel Polish with Manicure", TreatmentType = TreatmentType.GelPolish, Duration = 45, Price = 25 },
              new Treatment() { TreatmentName = "Pedicure", TreatmentType = TreatmentType.NaturalNail, Duration = 45, Price = 22 }
            };

            return treatments;
        }

        public List<EmployeeTreatment> GetEmployeeTreatments()
        {
            List<EmployeeTreatment> employeeTreatments = new List<EmployeeTreatment>();

            for (int i = 1; i <= 3; i++)
            {
                var employee = _context.Employees.Single(x => x.EmployeeId == i);
                var treatment = _context.Treatments.ToList();
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

                if(i == 3)
                {
                    employeeTreatments.Add(
                        new EmployeeTreatment()
                        {
                            Employee = employee,
                            Treatment = treatment[5]
                        });
                }
            }

            return employeeTreatments;
        }

        public List<OperatingTimeEmployee> GetOperatingTimeEmployees()
        {
            List<OperatingTimeEmployee> operatingTimeEmployees = new List<OperatingTimeEmployee>();

            for (int i = 1; i <= 3; i++)
            {
                var employee = _context.Employees.Single(x => x.EmployeeId == i);
                var operatingTimes = _context.OperatingTimes.ToList();

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

            return operatingTimeEmployees;
        }

    }
}

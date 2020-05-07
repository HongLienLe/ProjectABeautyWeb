using System;
using System.Collections.Generic;
using DataMongoApi.Controllers.AdminController;
using DataMongoApi.Models;
using DataMongoApi.Service.InterfaceService;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;

namespace DataMongoApiTest.ControllerTest
{
    public class EmployeeControllerTest
    {
        private Mock<IEmployeeService> _employeeService;
        private Mock<ITreatmentService> _treatmentService;
        private EmployeeController _employeeController;

        [SetUp]
        public void SetUp()
        {
            _employeeService = new Mock<IEmployeeService>();
            _treatmentService = new Mock<ITreatmentService>();
        }


        [Test]
        public void Get_All_Employees_List_Return_200()
        {
            _employeeService.Setup(x => x.Get()).Returns(SeedEmployeeData());
            _employeeController = new EmployeeController(_employeeService.Object, _treatmentService.Object);

            var actual = _employeeController.Get() as ObjectResult;

            Assert.AreEqual(actual.StatusCode, 200);
            Assert.IsNotNull(actual.Value);
        }

        [Test]
        public void Get_With_No_Employees_Return_404()
        {
            _employeeService.Setup(x => x.Get()).Returns(new List<Employee>());
            _employeeController = new EmployeeController(_employeeService.Object, _treatmentService.Object);

            var actual = _employeeController.Get() as ObjectResult;
            Assert.AreEqual(actual.StatusCode, 404);
        }

        [Test]
        public void Create_Employee_Return_200()
        {
            var id = "123456789012345678901234";
            var employee = new Employee()
            {
                ID = id,
                Details = new EmployeeDetails()
                {
                    Name = "Hong",
                    Email = "H@mail.com"
                }
            };

            _employeeService.Setup(x => x.Create(It.IsAny<EmployeeDetails>())).Returns(employee);
            _employeeController = new EmployeeController(_employeeService.Object, _treatmentService.Object);

            var actual = _employeeController.Create(employee.Details) as ObjectResult;

            Assert.AreEqual(actual.StatusCode, 200);
            Assert.IsNotNull(actual.Value as Employee);
            Assert.AreEqual(actual.Value, employee);
            
        }

        [Test]
        public void Read_Employee_Return_200()
        {
            var id = "123456789012345678901234";
            var employee = new Employee()
            {
                ID = id,
                Details = new EmployeeDetails()
                {
                    Name = "Hong",
                    Email = "H@mail.com"
                }
            };

            _employeeService.Setup(x => x.Get(It.IsAny<string>())).Returns(employee);
            _employeeController = new EmployeeController(_employeeService.Object, _treatmentService.Object);

            var actual = _employeeController.Get(id) as ObjectResult;

            Assert.AreEqual(actual.StatusCode, 200);
            Assert.IsNotNull(actual.Value as Employee);
            Assert.AreEqual(actual.Value, employee);

        }

        [Test]
        public void Read_Employee_That_Does_Not_Exist_Return_404()
        {
            var id = "123456789012345678901234";
            _employeeService.Setup(x => x.Get(It.IsAny<string>())).Returns(new Employee());
            _employeeController = new EmployeeController(_employeeService.Object, _treatmentService.Object);

            var actual = _employeeController.Get(id) as ObjectResult;

            Assert.AreEqual(actual.StatusCode, 404);
            Assert.IsNull(actual.Value as Employee);
        }

        [Test]
        public void Update_Employee_Return_200()
        {
            var id = "123456789012345678901234";
            var employee = new Employee()
            {
                ID = id,
                Details = new EmployeeDetails()
                {
                    Name = "Hong",
                    Email = "H@mail.com"
                }
            };

            _employeeService.Setup(x => x.Update(It.IsAny<string>(), employee.Details));
            _employeeService.Setup(x => x.Get(It.IsAny<string>())).Returns(employee);
            _employeeController = new EmployeeController(_employeeService.Object, _treatmentService.Object);

            var actual = _employeeController.Update(id, employee.Details) as ObjectResult;
            var actualUpdated = _employeeController.Get(id) as ObjectResult;

            Assert.AreEqual(actual.StatusCode, 200);
            Assert.IsNotNull(actualUpdated.Value as Employee);
            Assert.AreEqual(actualUpdated.Value, employee);
        }

        [Test]
        public void Remove_Employee_Return_200()
        {
            var id = "123456789012345678901234";
            var employee = new Employee()
            {
                ID = id,
                Details = new EmployeeDetails()
                {
                    Name = "Hong",
                    Email = "H@mail.com"
                }
            };

            _employeeService.Setup(x => x.Remove(It.IsAny<string>()));
            _employeeService.Setup(x => x.Get(It.IsAny<string>())).Returns(employee);

            _employeeController = new EmployeeController(_employeeService.Object, _treatmentService.Object);

            var actual = _employeeController.Delete(id) as ObjectResult;

            Assert.AreEqual(actual.StatusCode, 200);
        }

        public List<Employee> SeedEmployeeData()
        {
            return new List<Employee>()
            {
                new Employee()
                {
                    ID = "1",
                    Details = new EmployeeDetails()
                    {
                        Name = "Employee1",
                        Email = "E1@mail.com"
                    }
                },
                new Employee()
                {
                    ID = "2",
                    Details = new EmployeeDetails()
                    {
                        Name = "Employee2",
                        Email = "E2@mail.com"
                    }
                },
                new Employee()
                {
                    ID = "3",
                    Details = new EmployeeDetails()
                    {
                        Name = "Employee3",
                        Email = "E3@mail.com"
                    }
                }
            };
        }
    }
}

using AccessDataApi.Data;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Itenso.TimePeriod;
using NUnit.Framework;
using Moq;
using AccessDataApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AcessDataAPITest.ControllerTest
{
    public class EmployeeControllerTest
    {
        // Get employee details by id
            //invalid 404

        [Test]
        public void Return404ForInvalidEmployeeIdGet()
        {
            var mockEmployeeRepo = new Mock<IEmployeeRepo>();

            mockEmployeeRepo.Setup(x => x.GetEmployee(1)).Returns((Employee)null);

            var controller = new EmployeeController(mockEmployeeRepo.Object);

            var response = controller.Get(1) as ObjectResult;

            Assert.IsTrue(404 == response.StatusCode);
        }

        //valid Employee Ok

        //Add Employee
            //bad add

        //updateemployee
            // invalid
            // valid

        //deleteemployee
            //invalid
            //valid

    }
}

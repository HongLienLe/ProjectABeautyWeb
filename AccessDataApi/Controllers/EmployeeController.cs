using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.Data;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccessDataApi.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private IEmployeeRepo _employeeRepo;

        public EmployeeController(IEmployeeRepo employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var employees = _employeeRepo.GetEmployees();

            if (employees == null)
                return NoContent();

            var responseEmployeeDetails = new List<EmployeeDetails>();

            foreach (var employee in employees)
            {
                responseEmployeeDetails.Add( new EmployeeDetails
                {
                    Id = employee.EmployeeId,
                    EmployeeName = employee.EmployeName,
                    Email = employee.Email
                    }
                );
            }

                return Ok(responseEmployeeDetails);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var employee = _employeeRepo.GetEmployee(id);

            if (employee == null)
            {
                return NotFound(employee);
            }

                return Ok(new EmployeeDetails() {
                    Id = employee.EmployeeId,
                    EmployeeName = employee.EmployeName,
                    Email = employee.Email });
        }

        // POST api/values
        [HttpPost("add")]
        public void Post([FromBody]EmployeeForm newEmployee)
        {
            var employee = new Employee()
            {
                EmployeName = newEmployee.EmployeeName,
                Email = newEmployee.Email
            };

            _employeeRepo.AddEmployee(employee);

        }

        // PUT api/values/5
        [HttpPost("{id}")]
        public void Post(int id, [FromBody]EmployeeForm UpdatedEmployee)
        {
            var employee = new Employee()
            {
                EmployeName = UpdatedEmployee.EmployeeName,
                Email = UpdatedEmployee.Email
            };

            _employeeRepo.UpdateEmployee(id, employee);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _employeeRepo.DeleteEmployee(id);
        }

    }

    
}

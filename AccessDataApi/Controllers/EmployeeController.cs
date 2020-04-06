using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.Data;
using AccessDataApi.Functions;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccessDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

                return Ok(employee);
        }

        // POST api/values
        [HttpPost("add")]
        public IActionResult Post([FromBody]EmployeeForm newEmployee)
        {
            if (!ModelState.IsValid)
                return BadRequest(newEmployee);

            var employee = CastTo.Employee(newEmployee);

            return Ok( _employeeRepo.AddEmployee(employee));

        }

        // PUT api/values/5
        [HttpPost("{id}")]
        public IActionResult Post(int id, [FromBody]EmployeeForm UpdatedEmployee)
        {
            if (ModelState.IsValid)
                return BadRequest(UpdatedEmployee);

            var employee = CastTo.Employee(UpdatedEmployee);

            if (employee == null)
                return BadRequest($"Employee Id: {id} does not exist, make a new one");

            return Ok(_employeeRepo.UpdateEmployee(id, employee));
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            var response = _employeeRepo.DeleteEmployee(id);

            if (response == null)
                return BadRequest($"Employee Id: {id} does not exist");

            return Ok(response);
        }

    }

    
}

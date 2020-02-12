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

        public EmployeeController(ApplicationContext context, IEmployeeRepo employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_employeeRepo.GetEmployees());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var employee = _employeeRepo.GetEmployee(id);

            if(employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // POST api/values
        [HttpPost("add")]
        public void Post([FromBody]Employee employee)
        {
            _employeeRepo.AddEmployee(employee);

        }

        // PUT api/values/5
        [HttpPost("{id}")]
        public void Post(int id, [FromBody]Employee employee)
        {
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

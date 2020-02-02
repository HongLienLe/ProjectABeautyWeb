using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.Data;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccessDataApi.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private EmployeeRepo _employeeRepo;

        public EmployeeController(ApplicationContext context)
        {
            _employeeRepo = new EmployeeRepo(context);
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

            return Ok(employee);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Employee employee)
        {
            _employeeRepo.AddEmployee(employee);

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Employee employee)
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

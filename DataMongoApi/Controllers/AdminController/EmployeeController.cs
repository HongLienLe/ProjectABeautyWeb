using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using DataMongoApi.Service;
using DataMongoApi.Service.InterfaceService;
using DataMongoApi.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DataMongoApi.Controllers.AdminController
{
    [Route("admin/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService )
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var employees = _employeeService.Get();

            if (employees.Count == 0)
                return Ok(new List<Employee>());

            return Ok(_employeeService.Get());
        }

        [HttpGet("{id:length(24)}")]
        public IActionResult Get(string id)
        {
            var employee = _employeeService.Get(id);

            if (employee == null)
            {
                return NotFound(employee);
            }

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult Create([FromBody]EmployeeForm employee)
        {
            var response = _employeeService.Create(employee);

            if (response == null)
                return BadRequest();

            return Ok(response);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, [FromBody]EmployeeForm employeeIn)
        {
            var employee = _employeeService.Get(id);

            if (employee == null)
            {
                return NotFound();
            }

            _employeeService.Update(id, employeeIn);

            employee = _employeeService.Get(id);

            return Ok(employee);
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var employee = _employeeService.Get(id);

            if (employee == null)
            {
                return NotFound(employee);
            }

            _employeeService.Remove(employee.ID);

            return Ok(employee);
        }

    }
}

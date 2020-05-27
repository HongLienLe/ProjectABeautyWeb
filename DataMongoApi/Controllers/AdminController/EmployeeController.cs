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
        private readonly ITreatmentService _treatmentService;

        public EmployeeController(IEmployeeService employeeService, ITreatmentService treatmentService)
        {
            _employeeService = employeeService;
            _treatmentService = treatmentService;
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
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult Create([FromBody]EmployeeDetails employee)
        {
           
            var response = _employeeService.Create(employee);

            return Ok(response);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, [FromBody]EmployeeDetails employeeIn)
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
                return NotFound();
            }

            _employeeService.Remove(employee.ID);

            return Ok();
        }

        [HttpPost("{id:length(24)}/manage/treatment")]
        public IActionResult UpdateTreatment(string id, [Required ,FromBody]List<string> ids)
        {

            if (_employeeService.Get(id) == null)
                return NotFound($"Employee {id} does not exist");

            if (ids.Any(x => x.Count() != 24))
                return BadRequest($"Contains an Id that is not Length 24");

            if(ids.Any(x => _treatmentService.Get(x) == null))
                return NotFound($"Treatment ids contain {id} does not exist");

            _employeeService.AddTreatmentsSkills(id, ids);

            return Ok();
        }

        [HttpPost("{id:length(24)}/manage/workdays")]
        public IActionResult UpdateWorkDays(string id, [Required, FromBody]List<string> ids)
        {
            if (_employeeService.Get(id) == null)
                return NotFound($"Employee {id} does not exist");

            _employeeService.AddWorkDays(id, ids);

            return Ok();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AccessDataApi.Data;
using AccessDataApi.Functions;
using AccessDataApi.HTTPModels;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessDataApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTreatmentController : ControllerBase
    {
        private IEmployeeTreatmentRepo _employeeTreatmentRepo;
        private IDoes _does;
        private IValidationErrorMessage _ErrorMessage;

        public EmployeeTreatmentController(IEmployeeTreatmentRepo employeeTreatmentRepo, IDoes does, IValidationErrorMessage validationErrorMessage)
        {
            _employeeTreatmentRepo = employeeTreatmentRepo;
            _does = does;
            _ErrorMessage = validationErrorMessage;

        }

        [HttpGet("get/treatments/by/employee/{id}")]
        public IActionResult GetTreatmentsByEmployee(int id)
        {
            if (!_does.EmployeeExist(id))
                return NotFound(_ErrorMessage.EmployeeNotFoundMessage(id));

            var treatments = _employeeTreatmentRepo.GetTreatmentsByEmployee(id);

            return Ok(treatments);
        }

        [HttpGet("get/employees/by/treatment/{id}")]
        public IActionResult GetEmployeesByTreatment(int id)
        {
            if (!_does.TreatmentExist(id))
                return NotFound(_ErrorMessage.TreatmentNotFoundMessage(id));

            var employees = _employeeTreatmentRepo.GetEmployeesByTreatment(id);

            return Ok(employees);
        }


        [HttpPost("employee/add/treatments")]
        public IActionResult AddTreatmentsToEmployeeId([FromBody] OneIdToManyIdForm et)
        {
            if (!_does.EmployeeExist(et.Id))
                return NotFound(_ErrorMessage.EmployeeNotFoundMessage(et.Id));

            if (!et.Ids.Any(x => _does.TreatmentExist(x)))
                return BadRequest("Contains a Treatment Id that does not exist");

            var response = _employeeTreatmentRepo.AddTreatmentsToEmployee(et);

            return Ok(response);
        }

        [HttpPost("treatment/add/employees")]
        public IActionResult AddTreatmentIdToEmployees([FromBody] OneIdToManyIdForm et)
        {
            if (!_does.TreatmentExist(et.Id))
                return NotFound(_ErrorMessage.TreatmentNotFoundMessage(et.Id));

            if (!et.Ids.Any(x => _does.EmployeeExist(x)))
                return BadRequest("Contains a Employee Id that does not exist");

            var response = _employeeTreatmentRepo.AddEmployeesToTreatment(et);

            return Ok(response);
        }

        [HttpDelete("remove/treatment/from/employees")]
        public IActionResult RemoveTreatmentIdFromEmployees([FromBody]OneIdToManyIdForm et )
        {
            if (!_does.TreatmentExist(et.Id))
                return NotFound(_ErrorMessage.TreatmentNotFoundMessage(et.Id));

            if (!et.Ids.Any(x => _does.EmployeeExist(x)))
                return BadRequest("Contains a Employee Id that does not exist");

            var response = _employeeTreatmentRepo.RemoveTreatmentFromEmployees(et);

            return Ok(response);
        }

        [HttpDelete("remove/employee/from/treatments")]
        public IActionResult RemoveEmployeeIdFromTreatments([FromBody]OneIdToManyIdForm et)
        {
            if (!_does.EmployeeExist(et.Id))
                return NotFound(_ErrorMessage.EmployeeNotFoundMessage(et.Id));

            if (!et.Ids.Any(x => _does.TreatmentExist(x)))
                return BadRequest("Contains a Treatment Id that does not exist");

            var response = _employeeTreatmentRepo.RemoveEmployeeFromTreatments(et);

            return Ok(response);
        }


    }
}

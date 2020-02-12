using System.Net;
using System.Net.Http;
using AccessDataApi.Data;
using AccessDataApi.HTTPModels;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Mvc;

namespace AccessDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTreatmentController : ControllerBase
    {
        private EmployeeTreatmentRepo _employeeTreatmentRepo;

        public EmployeeTreatmentController(ApplicationContext context)
        {
            _employeeTreatmentRepo = new EmployeeTreatmentRepo(context);
        }

        [HttpGet("{employeeId}")]
        public IActionResult Get(int employeeId)
        { 
            var employeeTreatments = _employeeTreatmentRepo.GetTreatmentsByEmployee(employeeId);


            if(employeeTreatments == null)
            {
                return NotFound();
            }


            return Ok(employeeTreatments);
        }

        [HttpPost("employee/treatments")]
        public void AddTreatments([FromBody] EmployeeTreatmentCrud employeeTreatment)
        {
            _employeeTreatmentRepo.AddEmployeeTreatment(employeeTreatment);
        }

        [HttpPost("treatment/add/employees")]
        public void AddEmployees([FromBody] EmployeeTreatmentCrud employeeTreatment)
        {
            _employeeTreatmentRepo.AddEmployeeTreatment(employeeTreatment);
        }

        [HttpDelete]
        public void Delete([FromBody]EmployeeTreatmentCrud employeeTreatment )
        {
            _employeeTreatmentRepo.RemoveEmployeeTreatment(employeeTreatment);
        }
    }
}

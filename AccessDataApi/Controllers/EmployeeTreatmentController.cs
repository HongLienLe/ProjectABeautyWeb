using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.Data;
using AccessDataApi.HTTPModels;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Http;
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
        // GET: api/EmployeeTreatment
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    //var employeeTreatment = _employeeTreatmentRepo.GetTreatmentsByEmployee
        //    //return new string[] { "value1", "value2" };
        //}

        // GET: api/EmployeeTreatment/5
        [HttpGet("{employeeId}")]
        public IActionResult Get(int employeeId)
        { 
            var employeeTreatments = _employeeTreatmentRepo.GetTreatmentsByEmployee(employeeId);

            return Ok(employeeTreatments);
        }

        // POST: api/EmployeeTreatment
        [HttpPost()]
        public void Post([FromBody] AddEmployeeTreatment employeeTreatment)
        {

            var employeeId = employeeTreatment.EmployeeId;
            var treatmentIds = employeeTreatment.TreatmentIds;

            _employeeTreatmentRepo.AddTreatmentToEmployee(employeeId, treatmentIds);
        }

        // PUT: api/EmployeeTreatment/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

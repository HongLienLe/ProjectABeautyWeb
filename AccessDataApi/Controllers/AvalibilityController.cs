using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.Data;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccessDataApi.Controllers
{
    [Route("api/[controller]")]
    public class AvalibilityController : Controller
    {
        private IAvalibilityRepo _avalibiliyRepo;
        private IEmployeeRepo _employeeRepo;

        public AvalibilityController(ApplicationContext context, IAvalibilityRepo avalibilityRepo, IEmployeeRepo employeeRepo)
        {
            _avalibiliyRepo = avalibilityRepo;

            _employeeRepo = employeeRepo;
        }


        [HttpGet("{year}/{month}/{day}")]
        public IActionResult Get(int year, int month, int day)
        {
            DateTime choosenDate = new DateTime(year, month, day);

            return Ok(_avalibiliyRepo.GetAvaliableTime(choosenDate));
        }

        // GET api/values/5
        [HttpGet("date/{year}/{month}/{day}/employee/{employeeId}")]
        public IActionResult GetAvalibilityByEmployeeId(int year,int month, int day, int employeeId)
        {
            DateTime choosenDate = new DateTime(year, month, day);

            var employee = _employeeRepo.GetEmployee(employeeId);

            var avalibleTimeGap = _avalibiliyRepo.GetAvalibilityByEmployee(choosenDate, employee);

            return Ok(avalibleTimeGap);
        }

        [HttpGet("date/{year}/{month}/{day}/treatment/{treatmentId}")]
        public IActionResult GetEmployeeByDateTreatment(int year, int month, int day, int treatmentId)
        {
            DateTime choosenDate = new DateTime(year, month, day);

            var employee = _avalibiliyRepo.GetEmployeesAvaliableByDateTreatment(choosenDate, treatmentId);

            return Ok(employee);
        }
    }
}

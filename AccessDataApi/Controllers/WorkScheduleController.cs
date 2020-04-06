using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.Functions;
using AccessDataApi.HTTPModels;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccessDataApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkScheduleController : Controller
    {
        private IWorkScheduleRepo _workScheduleRepo;

        public WorkScheduleController(IWorkScheduleRepo workScheduleRepo)
        {
            _workScheduleRepo = workScheduleRepo;
        }

        [HttpPost("add/work/schedule")]
        public IActionResult AddWorkSchedule([FromBody] WorkScheduleModel wsh)
        {
            var response = _workScheduleRepo.addWorkSchedule(wsh);

            if (response.StartsWith("D"))
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("get/work/schedule/employee/{employeeId}")]
        public IActionResult GetWorkScheduleByEmployeeId(int employeeId)
        {
            var workDays = _workScheduleRepo.GetEmployeeWorkSchedule(employeeId);

            if (workDays == null)
                return NotFound(workDays);

            List<OperatingTimeDetails> responseOperatingTimeDetails = new List<OperatingTimeDetails>();

            foreach (var day in workDays)
            {
                responseOperatingTimeDetails.Add(CastTo.OperatingTimeDetails(day));
            }
            return Ok(responseOperatingTimeDetails);
        }

        [HttpGet("get/employee/date/{dayId}")]
        public IActionResult GetEmployeeByWorkDay(int dayId)
        {
            var employees = _workScheduleRepo.GetEmployeeByWorkDay(dayId);

            if (employees == null)
                return NotFound(employees);

            List<EmployeeDetails> responseEmployeeDetails = new List<EmployeeDetails>();

            foreach (var employee in employees)
            {
                responseEmployeeDetails.Add(CastTo.EmployeeDetails(employee));
            }
            return Ok(responseEmployeeDetails);
        }
    }
}

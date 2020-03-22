using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.HTTPModels;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccessDataApi.Controllers
{
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
        public void AddWorkSchedule([FromBody] WorkScheduleModel wsh)
        {
            _workScheduleRepo.addWorkSchedule(wsh);
        }

        [HttpGet("get/work/schedule/employee/{employeeId}")]
        public IActionResult GetWorkScheduleByEmployeeId(int employeeId)
        {
            var workDays = _workScheduleRepo.GetEmployeeWorkSchedule(employeeId);

            if (workDays == null)
                return NotFound(workDays);

            List<OpeningTimeDetail> responseOperatingTimeDetails = new List<OpeningTimeDetail>();

            foreach (var day in workDays)
            {
                responseOperatingTimeDetails.Add(new OpeningTimeDetail()
                {
                    Id = day.Id,
                    Day = day.Day,
                    StartTime = day.StartTime.ToString(),
                    EndTime = day.EndTime.ToString(),
                    isOpen = day.isOpen
                });
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
                responseEmployeeDetails.Add(new EmployeeDetails
                {
                    Id = employee.EmployeeId,
                    EmployeeName = employee.EmployeName,
                    Email = employee.Email
                }
                );
            }
            return Ok(responseEmployeeDetails);
        }
    }
}

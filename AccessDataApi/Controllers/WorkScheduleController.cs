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
            return Ok(_workScheduleRepo.GetEmployeeWorkSchedule(employeeId));
        }

        [HttpGet("get/employee/by/dayofweek/{dayId}")]
        public IActionResult GetEmployeeByWorkDay(int dayId)
        {
            return Ok(_workScheduleRepo.GetEmployeeByWorkDay(dayId));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.Data;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Mvc;

namespace AccessDataApi.Controllers
{
    [Route("api/[controller]")]
    public class AvailabilityController : Controller
    {
        private IAvailabilityRepo _availbiliyRepo;

        public AvailabilityController(IAvailabilityRepo availbilityRepo)
        {
            _availbiliyRepo = availbilityRepo;
        }

        [HttpGet("{year}/{month}/{day}")]
        public IActionResult GetAvailableTime(int year, int month, int day)
        {
            DateTime choosenDate = new DateTime(year, month, day);

            var response = _availbiliyRepo.GetAvailableTime(choosenDate);

            if (response == null)
                return BadRequest(Response);
            return Ok(response);
        }


        [HttpGet("date/{year}/{month}/{day}/treatment/{treatmentId}")]
        public IActionResult GetWorkingEmployeesByDateAndTreatment(int year, int month, int day, int treatmentId)
        {
            DateTime choosenDate = new DateTime(year, month, day);
            var response = _availbiliyRepo.GetWorkingEmployeesByDateAndTreatment(choosenDate, treatmentId);

            if (response == null)
                return BadRequest(response);

            return Ok(response);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.Data;
using AccessDataApi.HTTPModels;
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
                return BadRequest("No avaliable time");
            return Ok(response);
        }


        [HttpPost("date/treatments")]
        public IActionResult GetWorkingEmployeesByDateAndTreatment([FromBody]TimeSlotForTreatmentForm timeSlotForTreatmentForm)
        {


            DateTime choosenDate = DateTime.Parse(timeSlotForTreatmentForm.Date);
            var response = _availbiliyRepo.GetAvailableTimeWithTreatment(choosenDate, timeSlotForTreatmentForm.Treatments);

            if (response == null)
                return BadRequest(response);

            return Ok(response);
        }
    }
}

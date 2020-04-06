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

            DateTime dateTime = new DateTime(year, month, day);

            if (dateTime < DateTime.Today)
                return BadRequest("Past date not available. Must choose today or future dates");

            var response = _availbiliyRepo.GetAvailableTime(dateTime);
             
            if (response == null)
                return BadRequest("No available time slots for given day");

            return Ok(response);
        }


        [HttpPost("date/treatments")]
        public IActionResult GetWorkingEmployeesByDateAndTreatment([FromBody]TimeSlotForTreatmentForm timeSlotForTreatmentForm)
        {
            if (timeSlotForTreatmentForm.Date < DateTime.Today)
                return BadRequest("Must choose today or future dates");

            var response = _availbiliyRepo.GetAvailableTimeWithTreatment(timeSlotForTreatmentForm.Date, timeSlotForTreatmentForm.Treatments);

            if (response == null)
                return BadRequest("No available time slots for given day");

            return Ok(response);
        }
    }
}

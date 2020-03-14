using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.Data;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccessDataApi.Controllers
{
    [Route("api/[controller]")]
    public class BookAppController : Controller
    {
        private IBookAppointment _bookAppRepo;

        public BookAppController(IBookAppointment bookAppRepo)
        {
            _bookAppRepo = bookAppRepo;
        }

        // POST api/values
        [HttpPost("date/{year}/{month}/{day}/book")]
        public IActionResult CreateAppointment(int year, int month, int day, [FromBody]AppointmentDetails bookApp)
        {
            DateTime dateTime = new DateTime(year, month, day);

            if (bookApp.Reservation.StartTime > DateTime.Today)
                return BadRequest("Past date not available");

            var response = _bookAppRepo.CreateAppointment(dateTime, bookApp);

            if (response.EndsWith("e"))
                return NotFound(response);

            if (response.StartsWith("T"))
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("AppointmentId/{Id}")]
        public IActionResult GetAppointmentById(int Id)
        {

            var response = _bookAppRepo.GetAppointment(Id);

            if (response == null)
                return NotFound(response);

            return Ok(response);
        }
    }
}

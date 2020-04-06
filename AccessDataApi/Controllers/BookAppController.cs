using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.Data;
using AccessDataApi.Functions;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("date/{year}/{month}/{day}/book")]
        public IActionResult CreateAppointment(int year, int month, int day, [FromBody]BookAppointmentForm bookApp)
        {
            DateTime dateTime = new DateTime(year, month, day);

            if (dateTime < DateTime.Today)
                return BadRequest("Past date not available");

            var response = _bookAppRepo.MakeAppointment(bookApp);

            if (response.EndsWith("e"))
                return NotFound(response);

            if (response.StartsWith("T"))
                return NotFound(response);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("AppointmentId/{Id}")]
        public IActionResult GetAppointmentById(int Id)
        {
            var response = _bookAppRepo.GetAppointment(Id);

            if (response == null)
                return NotFound(response);

            return Ok(CastTo.BookAppDetails(response));
        }

        [Authorize]
        [HttpGet("Admin/BookApp/date/{year}/{month}/{day}")]
        public IActionResult GetBookedAppByDate(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);

            var response = _bookAppRepo.GetBookedAppointmentByDay(date);

            if (response == null)
                return NotFound($"no app for date = {date}");

            return Ok(response);
        }

        [Authorize]
        [HttpPost("Update/{id}")]
        public IActionResult UpdateBookingApp(int id, [FromBody] BookAppointmentForm bookapp)
        {
            var response = _bookAppRepo.UpdateApppointment(id, bookapp);

            if (response.StartsWith("B"))
                return NotFound(response);

            if (response.StartsWith("R"))
                return BadRequest(response);

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("Date/{year}/{month}/{day}/Id/{id}")]
        public IActionResult DeleteBookedAppointment(int year, int month, int day, int id)
        {
            var date = new DateTime(year, month, day);

            var response = _bookAppRepo.DeleteAppointment(date, id);
            if (response == null)
                return NotFound("Does not exist");
            return Ok(response);
        }

        [Authorize]
        [HttpGet("Admin/BookApp/Date/{year}/{month}/{day}/Employee/{employeeId}")]
        public IActionResult GetBookAppByEmployee(int year, int month, int day, int employeeId)
        {
            var date = new DateTime(year, month, day);
            var response = _bookAppRepo.GetBookAppByDateAndEmployee(date, employeeId);

            if (response == null)
                return NoContent();

            return Ok(response);
        }
    }
}

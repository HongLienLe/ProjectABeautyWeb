using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itenso.TimePeriod;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Appointment.Controllers
{
    [Route("api/Book")]
    public class BookingAppointmentController : Controller
    {

        BookingAppointment BookingAppointment;

        public BookingAppointmentController()
        {
            BookingAppointment = new BookingAppointment();
        }

        // GET: api/values
        [HttpPost("{app}")]
        public IActionResult BookApp(TimeRange app)
        {
            var requestedApp = BookingAppointment.BookAppointment(app);

            if (requestedApp == null)
            {
                return Ok("Booking is not confirmed");
            }

            return CreatedAtAction(nameof(ConfirmedApp),requestedApp);
        }

        [HttpGet("Confirmed/{app}")]
        public IActionResult ConfirmedApp(TimeRange app)
        {
            return Ok($"Booking for {app} is confirmed");
        }

    }
}

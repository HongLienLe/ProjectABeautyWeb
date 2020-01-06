using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itenso.TimePeriod;
using Microsoft.AspNetCore.Mvc;

namespace Appointment.Controllers
{
    [Route("api/[controller]")]
    public class AvaliableAppointmentController : Controller
    {
        AvaliableAppointment AvaliableAppointment;

        public AvaliableAppointmentController()
        {
            AvaliableAppointment = new AvaliableAppointment();
        }


        [HttpGet] // Get all current Keys
        public IActionResult Get()
        {
            var allKeys = AvaliableAppointment.Reservations.Keys;

            if (allKeys == null)
            {
                return Ok("Empty Keys");
            }

            return Ok(allKeys);
        }

        [HttpGet("Month/{month}")]//Get avaliable days in the month
        public IActionResult GetMonth(int month)
        {
            var choosenMonth = AvaliableAppointment.AvaliableDatesForMonth(month);

            if(choosenMonth == null)
            {
                return Ok("Month Not Avaliable");
            }

            return Ok(choosenMonth);
        }


        [HttpGet("Date/{year}/{month}/{day}")] // get avaliable times for requested date
        public IActionResult Get(int year, int month, int day)
        {
            DateTime requestedDay = new DateTime(year, month, day);
            var avaliableReservations = AvaliableAppointment.AvaliableAppDate(requestedDay);

            if(avaliableReservations == null)
            {
                return Ok("Day is not Avaliable");
            }

            return Ok(avaliableReservations);
        }




        //[HttpGet("{id}")]
        //public ITimePeriod ConfirmBooking(TimeRange bookedApp)
        //{
        //    var appBookings = AvaliableAppointment.AvaliableAppDate(bookedApp.Start.Date);
        //    var bookAppConfirm = appBookings.IndexOf(appBookings);
        //    return appBookings[bookAppConfirm];
        //}

        // POST api/values
        //[HttpPost]
        //public IActionResult Post([FromBody]TimeRange app)
        //{
           

        //  var confirmApp =  AvaliableAppointment.BookAppointment(app);

        //    if(confirmApp == null)
        //    {
        //        return Content("Not Avaliable");
        //    }

        //    return CreatedAtAction(nameof(ConfirmBooking), new { id = app.GetDescription() }, app);
        //}

    }
}

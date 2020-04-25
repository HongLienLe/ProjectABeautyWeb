using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMongoApi.Models;
using DataMongoApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace DataMongoApi.Controllers.ClientController
{
    [Route("[controller]")]
    public class BookController : Controller
    {
        private AppointmentService _appointmentService;

        public BookController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public IActionResult Post([FromBody]AppointmentDetails app)
        {
            var response = _appointmentService.ProcessAppointment(app);
            if (response == null)
                return BadRequest();

            return Ok(response);    
        }

        [HttpGet("{date}")]
        public IActionResult Get(DateTime date)
        {
            var response = _appointmentService.GetAppointments(date);
            if (response == null)
                return NoContent();

            return Ok(response);
        }

    }
}

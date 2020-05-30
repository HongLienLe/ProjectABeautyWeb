using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMongoApi.Models;
using DataMongoApi.Service;
using DataMongoApi.Service.InterfaceService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataMongoApi.Controllers.ClientController
{

    [Route("book")]
    public class BookController : Controller
    {
        private IAppointmentService _appointmentService;

        public BookController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public IActionResult Post([FromBody]AppointmentDetails app)
        {
            var response = _appointmentService.ProcessAppointment(app);
            if (response == null)
                return Ok("Appointment was not created");

            return Ok(response);    
        }

       // [Authorize]
        [HttpGet("{date}")]
        public IActionResult Get(string date)
        {
            var response = _appointmentService.GetAppointments(date);
            if (response == null)
                return NoContent();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _appointmentService.Remove(id);

            return Ok("");
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id,[FromBody] AppointmentDetails app)
        {
            var response = _appointmentService.UpdateAppointment(id, app);

            if (response == null)
                return BadRequest();

            return Ok(response);

        }
    }
}

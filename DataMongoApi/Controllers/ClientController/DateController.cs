using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMongoApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace DataMongoApi.Controllers.ClientController
{
    [Route("client/[controller]")]
    public class DateController : Controller
    {
        private AvailableAppointmentService _availableAppointmentService;

        public DateController(AvailableAppointmentService availableAppointmentService)
        {
            _availableAppointmentService = availableAppointmentService;
        }

        [HttpGet("{date}/treatment/{id}")]
        public IActionResult Get(DateTime date, string id)
        {
            return Ok(_availableAppointmentService.GetAvailableTimeSlot(date, id));
        }
    }
}

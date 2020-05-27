using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMongoApi.Models;
using DataMongoApi.Service;
using DataMongoApi.Service.InterfaceService;
using Microsoft.AspNetCore.Mvc;

namespace DataMongoApi.Controllers.ClientController
{
    [Route("[controller]")]
    public class AvailabilityController : Controller
    {
        private IAvailableAppointmentService _availableAppointmentService;

        public AvailabilityController(IAvailableAppointmentService availableAppointmentService)
        {
            _availableAppointmentService = availableAppointmentService;
        }

        [HttpPost()]
        public IActionResult Get([FromBody] AvailableAppRequestForm appRequestForm)
        {
            var times = _availableAppointmentService.GetAvailableTimeSlot(appRequestForm.DateTime, appRequestForm.TreatmentIds);

            if( times.Count == 0)
            {
                return Ok(new List<Appointment>());
            }

            return Ok(times);
        }
            
    }
}

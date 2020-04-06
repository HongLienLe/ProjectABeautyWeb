using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMongoApi.Models;
using DataMongoApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace DataMongoApi.Controllers
{
    [Route("api/[controller]")]
    public class OperatingHoursController : Controller
    {
        private readonly OperatingHoursService _operatingHoursService;

        public OperatingHoursController(OperatingHoursService operatingHoursService)
        {
            _operatingHoursService = operatingHoursService;
        }

        [HttpGet]
        public ActionResult<List<OperatingHours>> Get() =>
            _operatingHoursService.Get();

        [HttpGet("{id:length(24)}", Name = "GetOperatingDay")]
        public ActionResult<OperatingHours> Get(string dayId)
        {
            var day = _operatingHoursService.Get(dayId);

            if (day == null)
            {
                return NotFound();
            }

            return day;
        }

        [HttpPost]
        public ActionResult<OperatingHours> Create(OperatingHours opHrs)
        {
            _operatingHoursService.Create(opHrs);

            return CreatedAtRoute("GetOperatingDat", new { id = opHrs.Day.ToString() }, opHrs);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string dayId, OperatingHours opHrsIn)
        {
            var opHrs = _operatingHoursService.Get(dayId);

            if (opHrs == null)
            {
                return NotFound();
            }

            _operatingHoursService.Update(dayId, opHrsIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string dayId)
        {
            var opHrs = _operatingHoursService.Get(dayId);

            if (opHrs == null)
            {
                return NotFound();
            }

            _operatingHoursService.Remove(opHrs.Day);

            return NoContent();
        }
    }
}
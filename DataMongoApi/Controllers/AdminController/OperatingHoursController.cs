using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMongoApi.Models;
using DataMongoApi.Service;
using DataMongoApi.Service.InterfaceService;
using Microsoft.AspNetCore.Mvc;

namespace DataMongoApi.Controllers.AdminController
{
    [Route("admin/[controller]")]
    [ApiController]
    public class OperatingHoursController : Controller
    {
        private readonly IOperatingHoursService _operatingHoursService;

        public OperatingHoursController(IOperatingHoursService operatingHoursService)
        {
            _operatingHoursService = operatingHoursService;
        }

        [HttpGet]
        public IActionResult Get() =>
            Ok(_operatingHoursService.Get());

        [HttpGet("{id}")]
        public IActionResult Get(string Id)
        {
            var day = _operatingHoursService.Get(Id);

            if (day == null)
            {
                return NotFound();
            }

            return Ok(day);
        }

        [HttpPost]
        public IActionResult Create(OperatingHoursDetails opHrs)
        {
            var ophr = new OperatingHours()
            {
                About = opHrs
            };

            _operatingHoursService.Create(ophr);

            return Ok(ophr);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string Id, OperatingHoursDetails opHrsIn)
        {
            var opHrs = _operatingHoursService.Get(Id);

            if (opHrs == null)
            {
                return NotFound();
            }

            _operatingHoursService.Update(Id, opHrsIn);

            return Ok("Update Successful");
        }

        //[HttpDelete("{id:length(24)}")]
        //public IActionResult Delete(string dayId)
        //{
        //    var opHrs = _operatingHoursService.Get(dayId);

        //    if (opHrs == null)
        //    {
        //        return NotFound();
        //    }

        //    _operatingHoursService.Remove(opHrs.ID);

        //    return NoContent();
        //}
    }
}
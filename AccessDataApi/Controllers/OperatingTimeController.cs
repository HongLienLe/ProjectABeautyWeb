using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.Data;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccessDataApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OperatingTimeController : ControllerBase
    {
        private OperatingTimeRepo _operatingTimeRepo;

        public OperatingTimeController(ApplicationContext context)
        {
            _operatingTimeRepo = new OperatingTimeRepo(context);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var openingHours = _operatingTimeRepo.GetOperatingTimes().ToList();
            return Ok(openingHours);
        }

        // GET: api/OperatingTimeRepo/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var openingTimeForDay = _operatingTimeRepo.GetOperatingTime(id);

            if (openingTimeForDay == null)
                return NotFound(openingTimeForDay);

            return Ok(openingTimeForDay);
        }

        // POST: api/OperatingTimeRepo
        [HttpPost("{id}")]
        public IActionResult Post(int id, [FromBody] OperatingTime operatingTime)
        {
            var response =_operatingTimeRepo.UpdateOperatingTime(id, operatingTime);

            if (response.StartsWith("D"))
                return NotFound(response);

            if (response.StartsWith("e"))
                return BadRequest(response);

            return Ok(response);
        }
    }
}

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

            return Ok(openingTimeForDay);
        }

        // POST: api/OperatingTimeRepo
        [HttpPost("{id}")]
        public void Post(int id, [FromBody] OperatingTime operatingTime)
        {
            _operatingTimeRepo.UpdateOperatingTime(id, operatingTime);
        }
    }
}

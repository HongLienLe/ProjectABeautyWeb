using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.Data;
using AccessDataApi.Functions;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccessDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperatingTimeController : ControllerBase
    {
        private IOperatingTimeRepo _operatingTimeRepo;

        public OperatingTimeController(IOperatingTimeRepo operatingTimeRepo)
        {
            _operatingTimeRepo = operatingTimeRepo;
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
            var day = _operatingTimeRepo.GetOperatingTime(id);

            if (day == null)
                return NotFound("Out of range, ust be between 1-7");            

            return Ok(day);
        }

        [Authorize]
        [HttpPost("{id}")]
        public IActionResult Post(int id, [FromBody] OperatingTimeForm operatingTimeForm)
        {
            var operatingTime = CastTo.OperatingTime(operatingTimeForm);

            if (operatingTime == null)
                return BadRequest("StartTime or EndTime not in the correct formatt HH:MM:SS");

            var response =_operatingTimeRepo.UpdateOperatingTime(id, operatingTime);

            if (response.StartsWith("D"))
                return NotFound(response);

            if (response.StartsWith("E"))
                return BadRequest(response);

            return Ok(response);
        }
    }

   
}

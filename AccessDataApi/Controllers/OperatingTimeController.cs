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

            var openingTimesDetails = new List<OpeningTimeDetail>();

            foreach(var day in openingHours)
            {
                openingTimesDetails.Add(
                    new OpeningTimeDetail()
                    {
                        Id = day.Id,
                        Day = day.Day,
                        StartTime = day.StartTime.ToString(),
                        EndTime = day.EndTime.ToString(),
                        isOpen = day.isOpen
                    });
            }
            return Ok(openingTimesDetails);
        }

        // GET: api/OperatingTimeRepo/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var day = _operatingTimeRepo.GetOperatingTime(id);

            if (day == null)
                return NotFound(day);

            var choosenDay = new OpeningTimeDetail()
            {
                Id = day.Id,
                Day = day.Day,
                StartTime = day.StartTime.ToString(),
                EndTime = day.EndTime.ToString(),
                isOpen = day.isOpen
            };

            return Ok(choosenDay);
        }

        // POST: api/OperatingTimeRepo
        [HttpPost("{id}")]
        public IActionResult Post(int id, [FromBody] OpeningTimeForm operatingTimeForm)
        {
            var operatingTime = new OperatingTime()
            {
                StartTime = TimeSpan.Parse(operatingTimeForm.StartTime),
                EndTime = TimeSpan.Parse(operatingTimeForm.EndTime),
                isOpen = operatingTimeForm.isOpen

            };

            var response =_operatingTimeRepo.UpdateOperatingTime(id, operatingTime);

            if (response.StartsWith("D"))
                return NotFound(response);

            if (response.StartsWith("e"))
                return BadRequest(response);

            return Ok(response);
        }
    }

    public class OpeningTimeDetail : OpeningTimeForm
    {
        public int Id { get; set; }
        public string Day { get; set; }

    }
    
    public class OpeningTimeForm
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool isOpen { get; set; }
    }
}

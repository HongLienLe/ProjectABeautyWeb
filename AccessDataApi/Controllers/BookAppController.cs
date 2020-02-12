using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.Data;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccessDataApi.Controllers
{
    [Route("api/[controller]")]
    public class BookAppController : Controller
    {
        private IBookAppRepo _bookAppRepo;

        public BookAppController(ApplicationContext context, IBookAppRepo bookAppRepo)
        {
            _bookAppRepo = bookAppRepo;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("date/{year}/{month}/{day}/book")]
        public IActionResult Post(int year, int month, int day, [FromBody]BookApp bookApp)
        {
            DateTime dateTime = new DateTime(year, month, day);

            return Ok(_bookAppRepo.CreateAppointment(dateTime, bookApp));
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.Data;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccessDataApi.Controllers
{
    [Route("api/[controller]")]
    public class TreatmentController : Controller
    {
        private ITreatmentRepo _treatmentRepo;

        public TreatmentController(ITreatmentRepo treatmentRepo)
        {
            _treatmentRepo = treatmentRepo;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_treatmentRepo.GetTreatments());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var treatment = _treatmentRepo.GetTreatment(id);

            if (treatment == null)
                return BadRequest(treatment);
            return Ok(treatment);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Treatment treatment)
        {
            _treatmentRepo.AddTreatment(treatment);

        }

        // PUT api/values/5
        [HttpPost("{id}")]
        public void Put(int id, [FromBody]Treatment treatment)
        {
            _treatmentRepo.UpdateTreatment(id,treatment);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
           var response = _treatmentRepo.DeleteTreatment(id);

            if (response == null)
                return NotFound(response);

            return Ok(response);
        }

        //get by type
    }
}

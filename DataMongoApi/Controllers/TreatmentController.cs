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
    [ApiController]
    public class TreatmentController : ControllerBase
    {
        private readonly TreatmentService _treatmentService;

        public TreatmentController(TreatmentService treatmentService)
        {
            _treatmentService = treatmentService;
        }

        [HttpGet]
        public ActionResult<List<Treatment>> Get() =>
            _treatmentService.Get();

        [HttpGet("{id:length(24)}", Name = "GetTreatment")]
        public ActionResult<Treatment> Get(string id)
        {
            var treatment = _treatmentService.Get(id);

            if (treatment == null)
            {
                return NotFound();
            }

            return treatment;
        }

        [HttpPost]
        public ActionResult<Treatment> Create(Treatment treatment)
        {
            _treatmentService.Create(treatment);

            return CreatedAtRoute("GetTreatment", new { id = treatment.Id.ToString() }, treatment);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Treatment treatmentIn)
        {
            var treatment = _treatmentService.Get(id);

            if (treatment == null)
            {
                return NotFound();
            }

            _treatmentService.Update(id, treatmentIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var treatment = _treatmentService.Get(id);

            if (treatment == null)
            {
                return NotFound();
            }

            _treatmentService.Remove(treatment.Id);

            return NoContent();
        }
    }
}

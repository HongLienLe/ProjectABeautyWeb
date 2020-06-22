using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using DataMongoApi.Service;
using DataMongoApi.Service.InterfaceService;
using DataMongoApi.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace DataMongoApi.Controllers.AdminController
{
    [Route("admin/[controller]")]
    [ApiController]
    public class TreatmentController : ControllerBase
    {
        private readonly ITreatmentService _treatmentService;

        public TreatmentController(ITreatmentService treatmentService)
        {
            _treatmentService = treatmentService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_treatmentService.Get());
        }

        [HttpGet("{id:length(24)}")]
        public IActionResult Get(string id)
        {
            var treatment = _treatmentService.Get(id);

            if (treatment == null)
            {
                return NotFound(id);
            }
            return Ok(treatment);
        }

        [HttpPost]
        public IActionResult Create([FromBody]TreatmentDetails treatmentForm)
        {
            var treatment = new Treatment()
            {
                About = treatmentForm
            };
            _treatmentService.Create(treatment);

            return Ok( treatment);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, [Required,FromBody] TreatmentDetails treatmentForm)
        {
            var treatment = _treatmentService.Get(id);

            if (treatment == null)
            {
                return NotFound(id);
            }

            _treatmentService.Update(id, treatmentForm);

            return Ok("Treatment has been updated");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var treatment = _treatmentService.Get(id);
            if (treatment == null)
                return BadRequest(treatment);

            _treatmentService.Remove(treatment.ID);

            return Ok("");
        }
    
    }
}

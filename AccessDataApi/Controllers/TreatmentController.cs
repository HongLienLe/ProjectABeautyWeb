using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.Data;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccessDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var treatments = _treatmentRepo.GetTreatments();

            if (treatments == null)
                return NotFound(treatments);

            List<TreatmentDetails> responseTreatmentDetails = new List<TreatmentDetails>();

            foreach(var treatment in treatments)
            {
                responseTreatmentDetails.Add(new TreatmentDetails()
                {
                    Id = treatment.TreatmentId,
                    TreatmentType = treatment.TreatmentType,
                    TreatmentName = treatment.TreatmentName,
                    Price = treatment.Price,
                    Duration = treatment.Duration
                });
            }

            return Ok(responseTreatmentDetails);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var treatment = _treatmentRepo.GetTreatment(id);

            if (treatment == null)
                return BadRequest(treatment);
            return Ok(new TreatmentDetails()
            {
                Id = treatment.TreatmentId,
                TreatmentType = treatment.TreatmentType,
                TreatmentName = treatment.TreatmentName,
                Price = treatment.Price,
                Duration = treatment.Duration
            });
        }

        // POST api/values
        [HttpPost("add")]
        public IActionResult AddingTreatment([FromBody]TreatmentForm treatmentForm)
        {
            var treatment = new Treatment()
            {
                TreatmentType = treatmentForm.TreatmentType,
                TreatmentName = treatmentForm.TreatmentName,
                isAddOn = treatmentForm.isAddOn,
                Price = treatmentForm.Price,
                Duration = treatmentForm.Duration
            };

           return Ok( _treatmentRepo.AddTreatment(treatment));

        }

        // PUT api/values/5
        [HttpPost("update/{id}")]
        public IActionResult UpdatingTreatment(int id, [FromBody]TreatmentForm treatmentForm)
        {
            var treatment = new Treatment()
            {
                TreatmentType = treatmentForm.TreatmentType,
                TreatmentName = treatmentForm.TreatmentName,
                isAddOn = treatmentForm.isAddOn,
                Price = treatmentForm.Price,
                Duration = treatmentForm.Duration
            };

            var response =  _treatmentRepo.UpdateTreatment(id,treatment);

            if (response == null)
                return NotFound(response);

            var returnTreatment = new TreatmentDetails()
            {
                Id = response.TreatmentId,
                TreatmentType = response.TreatmentType,
                isAddOn = response.isAddOn,
                TreatmentName = response.TreatmentName,
                Price = response.Price,
                Duration = response.Duration
            };

            return Ok(new { response = "Successfully updated Treatment", treatment = returnTreatment });

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult DeletingTreatment(int id)
        {
           var response = _treatmentRepo.DeleteTreatment(id);

            if (response == null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("type/{treatmentType}")]
        public IActionResult GetByTreatmentType(TreatmentType treatmentType)
        {
            var response = _treatmentRepo.GetTreatmentByType(treatmentType);

            if (response == null)
                return NotFound(response);

            var treatmentsByType = new List<TreatmentDetails>();

            foreach (var t in response)
            {
                treatmentsByType.Add(new TreatmentDetails()
                {
                    Id = t.TreatmentId,
                    TreatmentType = t.TreatmentType,
                    isAddOn = t.isAddOn,
                    TreatmentName = t.TreatmentName,
                    Price = t.Price,
                    Duration = t.Duration
                });
            }

            return Ok(treatmentsByType);
        }
    }
}

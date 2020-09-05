using System;
using DataMongoApi.Models;
using DataMongoApi.Service.InterfaceService;
using Microsoft.AspNetCore.Mvc;

namespace DataMongoApi.Controllers.AdminController
{
    [Route("admin/[controller]")]
    public class TreatmentTypeController : Controller
    {
        private ITreatmentTypeService _tTypeService;

        public TreatmentTypeController(ITreatmentTypeService treatmentTypeService)
        {
            _tTypeService = treatmentTypeService;
        }

        [HttpGet()]
        public IActionResult Get()
        {
            return Ok(_tTypeService.Get());
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var response = _tTypeService.Get(id);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _tTypeService.Delete(id);
        }

        [HttpPost()]
        public IActionResult Post([FromBody]TreatmentTypeEntry treatmentType)
        {
            var response = _tTypeService.Create(treatmentType);

            if (response == null)
                return BadRequest();

            return Ok(response);
        }
    }
}

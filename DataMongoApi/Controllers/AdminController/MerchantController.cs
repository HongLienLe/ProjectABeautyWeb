using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMongoApi.Models;
using DataMongoApi.Service;
using DataMongoApi.Service.InterfaceService;
using Microsoft.AspNetCore.Mvc;

namespace DataMongoApi.Controllers.AdminController
{
    [Route("admin/[controller]")]
    [ApiController]
    public class MerchantController : Controller
    {
        private readonly IMerchantService _merchantService;

        public MerchantController(IMerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        [HttpGet("{id:length(24)}", Name = "GetMerchant")]
        public ActionResult<Merchant> Get(string id)
        {
            var merchant = _merchantService.Get(id);

            if (merchant == null)
            {
                return NotFound();
            }

            return merchant;
        }

        [HttpPost]
        public ActionResult<Merchant> Create([FromBody]Merchant merchant)
        {
            _merchantService.Create(merchant);

            return CreatedAtRoute("GetMerchant", new { id = merchant.Id.ToString() }, merchant);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, [FromBody] Merchant merchantIn)
        {
            var merchant = _merchantService.Get(id);

            if (merchant == null)
            {
                return NotFound();
            }

            _merchantService.Update(id, merchantIn);

            return NoContent();
        }
    }
}

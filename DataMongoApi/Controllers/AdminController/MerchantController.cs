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

        [HttpGet()]
        public IActionResult Get()
        {
            var merchant = _merchantService.Get();

            if (merchant == null)
            {
                return NotFound(merchant);
            }

            return Ok(merchant);
        }


        [HttpPut()]
        public IActionResult Update([FromBody] Merchant merchantIn)
        {
              _merchantService.Update(merchantIn);

            var merchant = _merchantService.Get();
            return Ok(merchant);
        }

    }
}

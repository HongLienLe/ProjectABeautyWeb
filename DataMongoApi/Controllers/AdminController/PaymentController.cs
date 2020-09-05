using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMongoApi.Models;
using DataMongoApi.Service.InterfaceService;
using Microsoft.AspNetCore.Mvc;

namespace DataMongoApi.Controllers.AdminController
{
    [Route("admin/[controller]")]
    public class PaymentController : Controller
    {
        private IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var response = _paymentService.Get(id);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPost()]
        public IActionResult Post([FromBody]OrderEntry entry)
        {
            var response = _paymentService.ProcessPayment(entry);

            if (response == null)
                return BadRequest();
            return Ok(response);
        }

        [HttpPut("{id}")]
        public void Put(string id, [FromBody]OrderDetails order)
        {
            _paymentService.Update(id, order);
        }
            
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _paymentService.Remove(id);
        }
    }
}

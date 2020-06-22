using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMongoApi.Models;
using DataMongoApi.Service.InterfaceService;
using Microsoft.AspNetCore.Mvc;

namespace DataMongoApi.Controllers.AdminController
{
    [Route("[controller]")]
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
            return Ok(_paymentService.Get(id));
        }

        [HttpPost()]
        public IActionResult Post([FromBody]OrderDetails booking)
        {
            var response = _paymentService.ProcessAppointment(booking);
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

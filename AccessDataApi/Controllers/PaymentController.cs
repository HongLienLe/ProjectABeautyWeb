//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using AccessDataApi.Functions;
//using AccessDataApi.HTTPModels;
//using AccessDataApi.Repo;
//using Microsoft.AspNetCore.Mvc;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace AccessDataApi.Controllers
//{
//    [Route("api/[controller]")]
//    public class PaymentController : Controller
//    {

//        private IProcessPayment _processPayment;
//        private IDoes _does;

//        public PaymentController(IProcessPayment processPayment, IDoes does)
//        {
//            _processPayment = processPayment;
//            _does = does;
//        }

//        [HttpGet("{id}")]
//        public IActionResult GetPayment(int id)
//        {
//            if (!_does.PaymentIdExist(id))
//                return NotFound("Payment Id does not exist");

//            var response = _processPayment.GetPayment(id);

//            return Ok(CastTo.PaymentDetails(response));
//        }

//        [HttpPost("process")]
//        public IActionResult ProcessingPayment([FromBody] ProcessPaymentForm form)
//        {
//            var reciept = _processPayment.ProcessBookedAppointment(form);

//            return Ok(reciept);
//        }

//        // DELETE api/values/5
//        [HttpDelete("{id}")]
//        public void Delete(int id)
//        {
//        }
//    }
//}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Appointment.Models;
//using Itenso.TimePeriod;
//using Microsoft.AspNetCore.Mvc;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace Appointment.Controllers
//{
//    [Route("api/Book")]
//    public class BookingAppointmentController : Controller
//    {
//        Data reservationData;
//        BookingAppWithDetail BookedDetails;

//        public BookingAppointmentController()
//        {
//           reservationData = Data.Instance;
//           BookedDetails = new BookingAppWithDetail();
//        }

//        [HttpGet] //Get Booked Reservation Keys
//        public IActionResult Get()
//        {
//            StringBuilder reservationDetails = new StringBuilder();
//            foreach (var key in reservationData.getAllConfirmedAppointments().Keys)
//            {
//                reservationDetails.AppendLine($"Date Key = {key}");

//                foreach (var reservationInDateTimeKey in reservationData.getAllConfirmedAppointments()[key])
//                {
//                    reservationDetails.AppendLine("");
//                    reservationDetails.AppendLine($"DateTimeId = {reservationInDateTimeKey.DateTimeId}");
//                    reservationDetails.AppendLine($"Appointment Id = {reservationInDateTimeKey.Id} ");
//                   // reservationDetails.AppendLine($"Client Name = {reservationInDateTimeKey.ClientName} ");
//                    reservationDetails.AppendLine($"Employee Id = {reservationInDateTimeKey.EmployeeId} ");
//                    reservationDetails.AppendLine($"Treatment Id = {reservationInDateTimeKey.TreatmentId}  ");
//                    reservationDetails.AppendLine($"Reservation key is {reservationInDateTimeKey.Reservation}");
//                    reservationDetails.AppendLine($"Reservation = {reservationInDateTimeKey.Reservation}");
//                }
//            }

//            return Ok(reservationDetails.ToString());
//        }

//        //Get Reservation by date 
//        [HttpGet("GetReservationByDate/{year}/{month}/{day}")]
//        public IActionResult GetReservationByDate(int year, int month, int day)
//        {
//            var date = new DateTime(year, month, day).Date;

//            if (!reservationData.getAllConfirmedAppointments().ContainsKey(date))
//            {
//                return Ok("No existing reservation for current date");
//            }

//            var reservation = reservationData.getAllConfirmedAppointments()[date];

//            return Ok(reservation);
//        }

//        [HttpPost()]
//        //Get Booked Appointments for given date key
//        public IActionResult Post([FromBody]BookAppointmentDetail booking)
//        {

//            var bookedApp = BookedDetails.BookTreatment(booking);

//            if (bookedApp == null)
//            {
//                return Ok("Appointment is not Booked");
//            }

//            return Ok(bookedApp);
//        }

//        //Edit Data by creating it in its entirety
//        [HttpPut("update")]
//        public IActionResult Put([FromBody]EditBookAppointmentDetails editedDetail)
//        {
//            var updatedApp = BookedDetails.UpdateExistingBooking(editedDetail.DateTimeId, editedDetail.AppId,editedDetail.updatedDetails);

//            if(updatedApp == null)
//            {
//                return Ok("Booking does not exist, or requested changes not possible keep old details?");
//            }

//            return Ok(BookedDetails.GetBookAppointmentDetail(updatedApp.DateTimeId, updatedApp.Id));
//        }

//        [HttpGet("GetTimeSlotData")]
//        public IActionResult GetReservationData()
//        {
//            return Ok(reservationData.getAllReservations()[new DateTime(2020,1,1)]);
//        }

//        [HttpGet("GetConfirmationData")]
//        public IActionResult GetConfirmationData()
//        {
//            return Ok(reservationData.getAllConfirmedAppointments()[new DateTime(2020,1,1)]);
//        }
//    }
//}

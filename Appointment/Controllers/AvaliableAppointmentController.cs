//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Appointment.Models;
//using Itenso.TimePeriod;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;

//namespace Appointment.Controllers
//{
//    [Route("api/[controller]")]
//    public class AvaliableAppointmentController : Controller
//    {
//        ValidationCheck ValidationCheck;
//        Data ReservationData;
//        AvaliableAppointment AvaliableAppointment;
//        EmployeeAvalibility EmployeeAvalibility;

//        public AvaliableAppointmentController()
//        {
//            ValidationCheck = new ValidationCheck();
//            ReservationData = Data.Instance;
//            AvaliableAppointment = new AvaliableAppointment( ValidationCheck);
//            EmployeeAvalibility = new EmployeeAvalibility(AvaliableAppointment);
//        }

//        [HttpGet] // Get all current Keys
//        public IActionResult Get()
//        {
//            var allKeys = ReservationData.getAllConfirmedAppointments().Keys;

//            if (allKeys == null)
//            {
//                return Ok("Empty Keys");
//            }


//            return Ok(allKeys);
//        }

//        [HttpGet("Date/{year}/{month}")]//Get avaliable days in the month
//        public IActionResult GetAvalibleDateForMonth(int year, int month)
//        {
//            var choosenMonth = AvaliableAppointment.AvaliableDatesForMonth(year,month);

//            if(choosenMonth == null)
//            {
//                return Ok("Month Not Avaliable");
//            }

//            return Ok(choosenMonth);
//        }

//        [HttpGet("Date/{year}/{month}/{day}")] // get avaliable times for requested date
//        public IActionResult GetAvaliableTimeForDate(int year, int month, int day)
//        {
//            DateTime requestedDay = new DateTime(year, month, day);
//            var avaliableReservations = AvaliableAppointment.AvaliableAppDate(requestedDay);

//            if(avaliableReservations == null)
//            {
//                return Ok("Day is not Avaliable");
//            }

//            return Ok(avaliableReservations);
//        }

//        //Get Month Avalible dates for this employee
//        [HttpGet("Date/Employee/{employeeId}/{year}/{month}")]
//        public IActionResult GetAvalibilityByEmployeeIdForMonth(int employeeId,int year, int month)
//        {
//            var allMonthAvalibleDates = EmployeeAvalibility.EmployeeAvalibilityDateForMonth(employeeId, month);

//            if(allMonthAvalibleDates == null)
//            {
//                return Ok($"Employee id = {employeeId} is not working for the month");
//            }

//            return Ok(allMonthAvalibleDates);
//        }

//        [HttpGet("Date/Employee/{employeeId}/{year}/{month}/{day}")]
//        public IActionResult GetAvalibilityByEmployeeIdForDate(int employeeId, int year, int month, int day)
//        {
//            DateTime requestedDay = new DateTime(year, month, day);

//            if(!EmployeeAvalibility.isEmployeeWorking(employeeId, requestedDay))
//            {
//                return Ok("Not Working Today");
//            }

//            if (!ReservationData.getAllReservations().ContainsKey(requestedDay.Date))
//            {

//                return Ok(AvaliableAppointment.AvaliableAppDate(requestedDay));
//            }
//            var workingHours = ReservationData.getAllReservations()[requestedDay.Date];

//            var workHoursDate = EmployeeAvalibility.EmployeeAvalibilityTimeSlots(employeeId, workingHours);

//            return Ok(workHoursDate);
//        }
//    }
//}

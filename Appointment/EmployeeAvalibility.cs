//using System;
//using System.Collections.Generic;
//using Appointment.Models;
//using Itenso.TimePeriod;

//namespace Appointment
//{
//    public class EmployeeAvalibility
//    {
//        Data openingHours;
//        EmployeeData EmployeeData;
//        AvaliableAppointment AvaliableAppointment;
//        ValidationCheck validationCheck;

//        public EmployeeAvalibility(AvaliableAppointment avaliableAppointment)
//        {
//            openingHours = Data.Instance;
//            validationCheck = new ValidationCheck();
//            EmployeeData = new EmployeeData();
//            AvaliableAppointment = avaliableAppointment;
//        }

//        public TimePeriodCollection EmployeeAvalibilityDateForMonth(int employeeId, int month)
//        {
//            if (!doesEmployeeExist(employeeId))
//            {
//                return null;
//            }
//            var choosenEmployee = EmployeeData.ListOfEmployees[employeeId];
//            var UpdatedWorkDaysForAll =  AvaliableAppointment.AvaliableDatesForMonth(DateTime.Today.Year,month);

//            foreach(var openingWorkHours in UpdatedWorkDaysForAll)
//            {
//                if (choosenEmployee.OffDays.Contains(openingWorkHours.Start.DayOfWeek.ToString()))
//                {
//                    UpdatedWorkDaysForAll.Remove(openingWorkHours);
//                }
//            }

//            return UpdatedWorkDaysForAll;
//        }

//        public ITimePeriodCollection EmployeeAvalibilityTimeSlots(int employeeId, ITimePeriod requestedDate)
//        {
//            if (!doesEmployeeExist(employeeId))
//            {
//                return null;
//            }

//            var choosenEmployee = EmployeeData.ListOfEmployees[employeeId];

//            if (choosenEmployee.OffDays.Contains(requestedDate.Start.DayOfWeek.ToString()))
//            {
//                return null;
//            }

//            TimePeriodCollection employeeClientApp = new TimePeriodCollection();

//            if (openingHours.getAllConfirmedAppointments().ContainsKey(requestedDate.Start.Date))
//            {
//                 foreach(var confirmedBooking in openingHours.getAllConfirmedAppointments()[requestedDate.Start.Date])
//                {
//                    if(confirmedBooking.EmployeeId == choosenEmployee.Id)
//                    {
//                        employeeClientApp.Add(confirmedBooking.Reservation);
//                    }
//                }
//            }

//            var timegapcalculator = new TimeGapCalculator<TimeRange>(new TimeCalendar());

//            return timegapcalculator.GetGaps(employeeClientApp, GetTimeRange(requestedDate.Start.Date));

//        }

//        public bool isEmployeeWorking(int employeeId, DateTime requestedDay)
//        {
//            if (!doesEmployeeExist(employeeId))
//            {
//                return false;
//            }

//            var choosenEmployee = EmployeeData.ListOfEmployees[employeeId];

//            if (choosenEmployee.OffDays.Contains(requestedDay.DayOfWeek.ToString()))
//            {
//                return false;
//            }

//            return true;
//        }

//        //public bool doesEmployeeProvideTreatment(int employeeId, int treatmentId)
//        //{
//        //    var choosenEmployee = EmployeeData.ListOfEmployees[employeeId];

//        //    if (!choosenEmployee.Treatments.ContainsKey(treatmentId))
//        //    {
//        //        return false;
//        //    }

//        //    return true;
//        //}


//        public TimeRange GetTimeRange(DateTime dateTime)
//        {
//            var hourRange = openingHours.getWorkHours()[dateTime.DayOfWeek.ToString()];
//            var range = new TimeSpan(
//                hourRange.End.Hour - hourRange.Start.Hour,
//                hourRange.End.Minute - hourRange.Start.Minute,
//                hourRange.End.Second - hourRange.Start.Second);

//            return new TimeRange(hourRange.Start.ToDateTime(dateTime.Date), range);
//        }

//        public bool doesEmployeeExist(int employeeId)
//        {
//            if (!EmployeeData.ListOfEmployees.ContainsKey(employeeId))
//            {
//                return false;
//            }

//            return true;
//        }
//    }
//}

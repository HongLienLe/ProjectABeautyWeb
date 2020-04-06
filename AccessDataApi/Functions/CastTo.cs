using System;
using AccessDataApi.Models;
using AccessDataApi.HTTPModels;
using System.Collections.Generic;
using System.Linq;

namespace AccessDataApi.Functions
{
    public static class CastTo
    {
        public static EmployeeDetails EmployeeDetails(Employee employee)
        {
            return new EmployeeDetails()
            {
                Id = employee.EmployeeId,
                EmployeeName = employee.EmployeName,
                Email = employee.Email
            };
        }

        public static TreatmentDetails TreatmentDetails(Treatment treatment)
        {
            return new TreatmentDetails()
            {
                Id = treatment.TreatmentId,
                TreatmentType = treatment.TreatmentType,
                TreatmentName = treatment.TreatmentName,
                Price = treatment.Price,
                Duration = treatment.Duration
            };
        }

        public static BookedAppointmentDetails BookAppDetails(AppointmentDetails app)
        {
            return new BookedAppointmentDetails()
            {
                Id = app.AppointmentDetailsId,
                StartTime = app.Reservation.StartTime,
                EndTime = app.Reservation.EndTime,
                EmployeeId = app.EmployeeId,
                TreatmentType = app.Treatment.TreatmentType.ToString(),
                TreatmentName = app.Treatment.TreatmentName,
                Client = new ClientForm()
                {
                    FirstName = app.ClientAccount.FirstName,
                    LastName = app.ClientAccount.LastName,
                    Email = app.ClientAccount.Email,
                    ContactNumber = app.ClientAccount.ContactNumber
                }
            };
        }

        public static OperatingTimeDetails OperatingTimeDetails(OperatingTime day)
        {
            return new OperatingTimeDetails()
            {
                Id = day.Id,
                Day = day.Day,
                StartTime = day.StartTime.ToString(),
                EndTime = day.EndTime.ToString(),
                isOpen = day.isOpen
            };
        }

        public static OperatingTime OperatingTime(OperatingTimeForm operatingTimeForm)
        {
            TimeSpan startTime;
            TimeSpan endTime;

            if (!TimeSpan.TryParse(operatingTimeForm.StartTime, out startTime)
               ||
                !TimeSpan.TryParse(operatingTimeForm.EndTime, out endTime))
                return null;

            return new OperatingTime()
            {
                StartTime = startTime,
                EndTime = endTime,
                isOpen = operatingTimeForm.isOpen

            };
        }

        public static ClientAccount ClientAccount(ClientForm clientForm)
        {
            return new ClientAccount()
            {
                FirstName = clientForm.FirstName,
                LastName = clientForm.LastName,
                Email = clientForm.Email,
                ContactNumber = clientForm.ContactNumber
            };
        }

        public static ClientDetails ClientDetails(ClientAccount clientAccount)
        {
            return new ClientDetails()
            {
                Id = clientAccount.ClientAccountId,
                FirstName = clientAccount.FirstName,
                LastName = clientAccount.LastName,
                ContactNumber = clientAccount.ContactNumber,
                Email = clientAccount.Email,
            };
        }

        public static DateTime ChoosenDate(string date)
        {
            return DateTime.Parse(date);
        }

        public static Employee Employee(EmployeeForm employeeForm)
        {
            return new Employee()
            {
                EmployeName = employeeForm.EmployeeName,
                Email = employeeForm.Email
            };
        }

        public static PaymentDetails PaymentDetails(Payment payment)
        {
            return new PaymentDetails()
            {
                Id = payment.Id,
                PaymentTime = payment.PaymentTime,
                ClientId = payment.Id,
                HadMISC = payment.TotalMISCAmount == 0 ? false : true,
                Total = payment.TotalAmount
            };
        }
    }
}

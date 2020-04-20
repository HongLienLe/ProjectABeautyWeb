using System;
using System.Linq;
using AccessDataApi.Data;
using Microsoft.EntityFrameworkCore;

namespace AccessDataApi.Functions
{
    public class Does : IDoes
    {
       private ApplicationContext _context;


        public Does(ApplicationContext context)
        {
            _context = context;
        }

        public bool EmployeeExist(int id)
        {
            return _context.Employees.Any(x => x.EmployeeId == id);
        }

        public bool TreatmentExist(int id)
        {
            return _context.Treatments.Any(x => x.TreatmentId == id);
        }

        public bool isOperatingOpen(DateTime date)
        {
            return _context.OperatingTimes.Any(x => x.Id == (int)date.DayOfWeek && x.isOpen == true);
        }

        public bool ClientExist(int id)
        {
             return _context.ClientAccounts.Any(x => x.ClientAccountId == id);
        }

        public bool AppIdExist(int id)
        {
            return _context.AppointmentDetails.Any(x => x.AppointmentDetailsId == id);
        }

        public bool DateTimeKeyExist(DateTime date)
        {
            return _context.DateTimeKeys.Any(x => x.date == date);
        }

        public bool OpeningIdExist(int id)
        {
            return _context.OperatingTimes.Any(x => x.Id == id);
        }

        //public bool PaymentIdExist(int id)
        //{
        //    return _context.Payments.Any(x => x.Id == id);
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.Functions;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public class ProcessPayment : IProcessPayment
    {
        private ApplicationContext _context;
        private IBookAppointment _bookRepo;
        private IClientAccountRepo _clientRepo;
        private IDateTimeKeyRepo _dateTimeKeyRepo;
        private ITreatmentRepo _treatmentRepo;

        public ProcessPayment(ApplicationContext context, IBookAppointment bookRepo, IClientAccountRepo clientAccountRepo, IDateTimeKeyRepo dateTimeKeyRepo, ITreatmentRepo treatmentRepo)
        {
            _context = context;
            _bookRepo = bookRepo;
            _clientRepo = clientAccountRepo;
            _dateTimeKeyRepo = dateTimeKeyRepo;
            _treatmentRepo = treatmentRepo;
        }

        public PaymentDetails ProcessBookedAppointment(ProcessPaymentForm paymentForm)
        {
            var bookApp = paymentForm.bookedAppIds.Select(x => _bookRepo.GetAppointment(x)).ToList();
            var client = bookApp.Select(x => _clientRepo.GetClientAccount(x.ClientAccountId)).ToList()[0];
            var dateTimeKey = bookApp.Select(x => _dateTimeKeyRepo.GetDateTimeKey( DateTime.Parse(x.DateTimeKeyId))).ToList()[0];
            var totalTreatmentCost = bookApp.Select(x => _treatmentRepo.GetTreatment( x.TreatmentId).Price).Sum();


            var totalMISC = paymentForm.MiscAmount.Sum();

            var payment = new Payment()
            {
                PaymentTime = DateTime.Now,
                ClientAccount = client,
                DateTimeKey = dateTimeKey,
                Treatments = bookApp.Select(x => x.Treatment).ToList(),
                TotalMISCAmount = totalMISC,
                TotalAmount = (int)totalTreatmentCost
            };

            _context.Payments.Add(payment);

            _context.SaveChanges();

            return CastTo.PaymentDetails(payment);
        }

        public void SavePayment(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
        }

        public Payment GetPayment(int id)
        {
            if (!_context.Payments.Any(x => x.Id == id))
                return null;

            return _context.Payments.Single(x => x.Id == id);
        }

        public List<PaymentDetails> GetPaymentDetailsByDate(DateTime date)
        {
            if (!_context.Payments.Any(x => x.DateTimeKey.date == date))
                return null;

            return _context.Payments
                .Where(x => x.DateTimeKey.date == date)
                .Select(x => CastTo.PaymentDetails(x)).ToList();
        }
    }
}

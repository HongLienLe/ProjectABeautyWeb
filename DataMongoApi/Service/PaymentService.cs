using System;
using System.Collections.Generic;
using System.Linq;
using DataMongoApi.DbContext;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using DataMongoApi.Service.InterfaceService;
using MongoDB.Driver;

namespace DataMongoApi.Service
{
    public class PaymentService : IPaymentService
    {
        private IMongoCollection<OrderDetails> _orders { get; set; }
        private IMongoCollection<Appointment> _appointments { get; set; }
        private IMongoCollection<Client> _client { get; set; }
        private ITreatmentService _treatmentService;
        private readonly IMongoDbContext _context;

        public PaymentService(IMongoDbContext context, ITreatmentService treatmentService)
        {
            _context = context;
            _orders = _context.GetCollection<OrderDetails>("OrderDetails");
            _client = _context.GetCollection<Client>("Clients");
            _appointments = _context.GetCollection<Appointment>("Appointments");
            _treatmentService = treatmentService;
        }

        public OrderDetails ProcessPayment(OrderEntry entry)
        {
            ProcessAppointment(entry.AppointmentID);

            var treatmentOrder = new List<TreatmentOrder>();
            var totalPrice = 0;

            foreach (var treatment in entry.TreatmentIds)
            {
                var choosenTreatment = _treatmentService.Get(treatment);
                if (choosenTreatment == null)
                    return null;


                treatmentOrder.Add(new TreatmentOrder()
                {
                    TreatmentId = choosenTreatment.ID,
                    TreatmentName = $"{choosenTreatment.About.TreatmentType} {choosenTreatment.About.TreatmentName}",
                    Price = choosenTreatment.About.Price
                });

                totalPrice += choosenTreatment.About.Price;
            }
            var order = Create(new OrderDetails()
            {
                ClientId = entry.ClientId,
                TreatmentOrders = treatmentOrder,
                MiscPrice = entry.MiscPrice,
                Total = totalPrice,
                ModifiedOn = DateTime.Now
            });

            ProcessOrder(order);
            return order;
        }

        private void ProcessOrder(OrderDetails order)
        {
            var filter = Builders<Client>.Filter.Eq(c => c.ID, order.ClientId);
            var updateClient = Builders<Client>.Update
                .AddToSet("Orders", order.ID)
                .CurrentDate(x => x.ModifiedOn);
            _client.UpdateOne(filter, updateClient);
        }

        private void ProcessAppointment(string id)
        {
            var filter = Builders<Appointment>.Filter.Eq(x => x.ID, id);
            var update = Builders<Appointment>.Update
                .Set(x => x.HasBeenProcess, true)
                .CurrentDate(x => x.ModifiedOn);

            _appointments.UpdateOne(filter, update);
        }

        public OrderDetails Create(OrderDetails order)
        {
            order.ModifiedOn = DateTime.Now;
            _orders.InsertOne(order);
            return order;
        }

        public OrderDetails Get(string id) 
        {
            return _orders.Find<OrderDetails>(o => o.ID == id).FirstOrDefault();
        }

        public void Update(string id, OrderDetails orderDetails)
        {
            var filter = Builders<OrderDetails>.Filter.Eq(o => o.ID, id);
            var update = Builders<OrderDetails>.Update
                .Set(o => o.TreatmentOrders, orderDetails.TreatmentOrders)
                .Set(o => o.Total, orderDetails.Total)
                .CurrentDate(o => o.ModifiedOn);
            _orders.UpdateOne(filter, update);
        }

        public void Remove(string id) =>
            _orders.DeleteOne(o => o.ID == id);

        public void Remove(OrderDetails orderDetails) =>
            _orders.DeleteOne(o => o.ID == orderDetails.ID);
    }
}

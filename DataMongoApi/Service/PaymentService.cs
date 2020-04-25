using System;
using System.Collections.Generic;
using System.Linq;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using DataMongoApi.Service.InterfaceService;
using MongoDB.Driver;

namespace DataMongoApi.Service
{
    public class PaymentService
    {
        private IMongoCollection<OrderDetails> _orders { get; set; }
        private IMongoCollection<Client> _client { get; set; }
        private ITreatmentService _treatmentService { get; set; }

        public PaymentService(ISalonDatabaseSettings settings, IClientConfiguration clientConfiguration, ITreatmentService treatmentService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(clientConfiguration.MerchantId);
            _orders = database.GetCollection<OrderDetails>("Orders");
            _client = database.GetCollection<Client>("Clients");
            _treatmentService = treatmentService;
        }

        public string ProcessAppointment(Appointment bookings)
        {
            var treatments = new List<TreatmentOrder>();

            foreach (var treatmentId in bookings.Info.TreatmentId)
            {
                treatments.Add(new TreatmentOrder()
                {
                    TreatmentId = treatmentId,
                    Price = _treatmentService.Get(treatmentId).About.Price
                });
            }

            var order = new OrderDetails()
            {
                ClientPhone = bookings.Info.Client.Phone,
                Treatments = treatments   
            };

            var processOrder = Create(order);

            var filter = Builders<Client>.Filter.Eq(c => c.About.Phone, bookings.Info.Client.Phone);
            var updateClient = Builders<Client>.Update
                .AddToSet("Orders", processOrder.ID)
                .CurrentDate(x => x.ModifiedOn);

            _client.UpdateOne(filter, updateClient);

            return "Payment Done";
        }

        public OrderDetails Create(OrderDetails order)
        {
            order.ModifiedOn = DateTime.Now;
            order.Total = order.Treatments.Select(x => x.Price).Sum();
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
                .Set(o => o.Treatments, orderDetails.Treatments)
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

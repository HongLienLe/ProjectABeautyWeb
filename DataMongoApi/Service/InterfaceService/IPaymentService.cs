using System;
using DataMongoApi.Models;

namespace DataMongoApi.Service.InterfaceService
{
    public interface IPaymentService
    {
        public string ProcessAppointment(OrderDetails bookings);
        public OrderDetails Create(OrderDetails order);
        public OrderDetails Get(string id);
        public void Update(string id, OrderDetails orderDetails);
        public void Remove(string id);
        public void Remove(OrderDetails orderDetails);

    }
}

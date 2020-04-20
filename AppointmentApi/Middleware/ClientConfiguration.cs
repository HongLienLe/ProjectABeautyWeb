using System;
namespace AppointmentApi.Middleware
{
    public class ClientConfiguration : IClientConfiguration
    {
        public string MerchantId { get; set; }
    }

    public interface IClientConfiguration
    {
        public string MerchantId { get; set; }

    }
}

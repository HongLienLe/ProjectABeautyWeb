using System;
using MongoDB.Entities.Core;

namespace AppointmentApi.Models
{
    public class Client : Entity
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
    }
}

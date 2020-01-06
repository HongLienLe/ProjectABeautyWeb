using System;
namespace Appointment.Models
{
    public class ClientAccount
    {
        public Guid Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }


        public ClientAccount()
        {
        }
    }
}

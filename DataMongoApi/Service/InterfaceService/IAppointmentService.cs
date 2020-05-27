using System;
using System.Collections.Generic;
using DataMongoApi.Models;

namespace DataMongoApi.Service.InterfaceService
{
    public interface IAppointmentService
    {
        public Appointment ProcessAppointment(AppointmentDetails app);
        public List<ReadAppointment> GetAppointments(string date);
        public void Remove(string appointmentId);
        public Appointment UpdateAppointment(string id, AppointmentDetails app);
    }
}

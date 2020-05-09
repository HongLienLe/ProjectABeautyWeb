﻿using System;
using System.Collections.Generic;
using DataMongoApi.Models;

namespace DataMongoApi.Service.InterfaceService
{
    public interface IAppointmentService
    {
        public Appointment ProcessAppointment(AppointmentDetails app);
        public List<Appointment> GetAppointments(string date);
        public void Remove(string appointmentId);
    }
}

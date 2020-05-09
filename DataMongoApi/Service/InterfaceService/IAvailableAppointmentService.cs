using System;
using System.Collections.Generic;

namespace DataMongoApi.Service.InterfaceService
{
    public interface IAvailableAppointmentService
    {
        public List<DateTime> GetAvailableTimeSlot(DateTime date, List<string> treatmentId);

    }
}

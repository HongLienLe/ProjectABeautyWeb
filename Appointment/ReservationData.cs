using System;
using System.Collections.Generic;
using Itenso.TimePeriod;

namespace Appointment
{
    public sealed class Data
    {
        static readonly Data ReservationData = new Data();
        public static Data Instance { get { return ReservationData; }}

        private Dictionary<DateTime, TimePeriodCollection> Reservations;
        private Dictionary<DateTime, List<BookAppointmentDetail>> BookedReservations;
        private Dictionary<string, HourRange> OpeningHours;

        private Data()
        {
            Reservations = new Dictionary<DateTime, TimePeriodCollection>()
            { {new DateTime(2020,1,1), new TimePeriodCollection(){ new TimeRange(new DateTime(2020,1,1,10,0,0), new TimeSpan(0,45,0)) } } };

            BookedReservations = new Dictionary<DateTime, List<BookAppointmentDetail>>()
            {
                {new DateTime(2020,1,1), new List<BookAppointmentDetail>(){
                    new BookAppointmentDetail()
                    {
                        Id = 1,
                        ClientName = "TestClient",
                        EmployeeId = 1,
                        TreatmentId = 0,
                        DateTimeId = new DateTime(2020,1,1),
                        Reservation = new TimeRange(new DateTime(2020,1,1,10,0,0), new TimeSpan(0,45,0))
                    }
                    }
                }
            };

            OpeningHours = new Dictionary<string, HourRange>()
        {
            {"Sunday", new HourRange(0,0) },
            {"Monday", new HourRange(10,19)},
            {"Tuesday", new HourRange(10,19) },
            {"Wednesday", new HourRange(10,19) },
            {"Thursday",  new HourRange(10,19)},
            {"Friday", new HourRange(10, 19)},
            {"Saturday", new HourRange(10,19)}
        };
        }

        public Dictionary<DateTime, TimePeriodCollection> getAllReservations()
        {
            return Reservations;
        }

        public Dictionary<DateTime, List<BookAppointmentDetail>> getAllConfirmedAppointments()
        {
            return BookedReservations;
        }

        public Dictionary<string, HourRange> getWorkHours()
        {
            return OpeningHours;
        }
    }
}
